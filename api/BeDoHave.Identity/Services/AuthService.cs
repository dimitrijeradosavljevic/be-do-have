using BeDoHave.Identity.Constants;
using BeDoHave.Identity.Models;
using BeDoHave.Identity.Options;
using BeDoHave.Shared.Dtos;
using BeDoHave.Shared.Exceptions;
using BeDoHave.Shared.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeDoHave.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityAppUser> _userManager;
        private readonly JwtOption _jwtOption;
        private readonly ITokenService _tokenService;
        private readonly IDataProtector _dataProtector;

        public AuthService(
            UserManager<IdentityAppUser> userManager,
            IOptions<JwtOption> jwtOption,
            ITokenService tokenService,
            IDataProtectionProvider dataProtectionProvider)
        {
            _userManager = userManager;
            _jwtOption = jwtOption.Value;
            _tokenService = tokenService;
            _dataProtector = dataProtectionProvider.CreateProtector(_jwtOption.SigningKey);
        }

        public async Task<string> CreateIdentityAsync(RegisterDto registerDto)
        {
            var user = new IdentityAppUser { UserName = registerDto.Email, Email = registerDto.Email};
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var message = result.Errors
                    .Select(e => e.Description)
                    .Aggregate((current, next) => $"{current} {next}");

                throw new ApiException(message, 500);
            }

            return user.Id;
        }

        public async Task DeleteIdentityAsync(string identityUserId)
        {
            var identityUser = await _userManager.FindByIdAsync(identityUserId);

            if (identityUser is null)
            {
                throw new ApiException($"User with id: {identityUserId} not found", 404);
            }

            await _userManager.DeleteAsync(identityUser);
        }

        public async Task<string> FindIdByNameAsync(string userName)
        {
            var identityUser = await _userManager.FindByNameAsync(userName);

            if (identityUser is null)
            {
                throw new ApiException($"User with username: {userName} not found", 404);
            }

            return identityUser.Id;
        }

        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var identityUser = await _userManager.FindByNameAsync(forgotPasswordDto.Email);

            if (identityUser is null)
            {
                throw new ApiException($"User with email {forgotPasswordDto.Email} not found", 404);
            }

            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
            var protectedToken = _dataProtector.Protect($"{forgotPasswordDto.Email}:~:{passwordResetToken}");
            var url = $"/api/accounts/{protectedToken}";

            //send email with token
            return url;
        }

        public async Task<bool> IsAdmin(string identityUserId)
        {
            var identityUser = await _userManager.FindByIdAsync(identityUserId);

            if (identityUser is null)
            {
                throw new ApiException($"User with id: {identityUserId} notFound", 404);
            }

            return await _userManager.IsInRoleAsync(identityUser, Roles.Admin);
        }

        public async Task<TokenDto> LoginAsync(LoginDto loginDto)
        {
            var identityUser = await _userManager
                .Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (identityUser is null)
            {
                throw new ApiException($"User with email: {loginDto.Email} not found", 404);
            }

            if (!await _userManager.CheckPasswordAsync(identityUser, loginDto.Password))
            {
                throw new ApiException("Password not valid", 400);
            }

            //if (!await _userManager.IsEmailConfirmedAsync(identityUser))
            //{
            //    throw new ApiException("Email not confirmed", 401);
            //}

            var roles = await _userManager.GetRolesAsync(identityUser);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var accessToken = _tokenService.GenerateAccessToken(identityUser.UserName, identityUser.Id, roles);

            if (identityUser.RefreshTokens is null)
            {
                identityUser.RefreshTokens = new List<RefreshToken>();
            }

            identityUser.RefreshTokens.Add(new RefreshToken { Token = refreshToken, Expires = _jwtOption.Expiration });
            await _userManager.UpdateAsync(identityUser);

            return new TokenDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Expiration = _jwtOption.Expiration
            };
        }

        public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var emailAndToken = _dataProtector.Unprotect(resetPasswordDto.Token).Split(":~:");

            if (emailAndToken.Length != 2)
            {
                throw new ApiException("Password reset token not valid", 400);
            }

            var identityUser = await _userManager.FindByNameAsync(emailAndToken[0]);

            if (identityUser is null)
            {
                throw new ApiException($"User with email: {emailAndToken[0]} not found", 404);
            }

            var result = await _userManager.ResetPasswordAsync(identityUser, emailAndToken[1], resetPasswordDto.Password);

            if (!result.Succeeded)
            {
                var message = result.Errors
                    .Select(e => e.Description)
                    .Aggregate((current, next) => $"{current}{next}");

                throw new ApiException(message, 500);
            }

        }

        public async Task<TokenDto> RevokeAsync(RevokeTokenDto revokeTokenDto)
        {
            var usernameClaim = _tokenService.GetUsernameClaimFromToken(revokeTokenDto.AccessToken);

            if (usernameClaim is null)
            {
                throw new ApiException("Access token not valid", 401);
            }

            var identityUser = await _userManager
                .Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.UserName == usernameClaim.Value);

            if (identityUser is null)
            {
                throw new ApiException("User with username: {usernameClaim.Value} not found", 404);
            }

            var refreshToken = identityUser.RefreshTokens.FirstOrDefault(rt => rt.Token == revokeTokenDto.RefreshToken && rt.Active);

            if (refreshToken is null)
            {
                throw new ApiException("Refresh token not valid", 401);
            }

            var roles = await _userManager.GetRolesAsync(identityUser);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            var newAccessToken = _tokenService.GenerateAccessToken(identityUser.UserName, identityUser.Id, roles);

            identityUser.RefreshTokens.Remove(refreshToken);
            identityUser.RefreshTokens.Add(new RefreshToken { Token = newRefreshToken, Expires = _jwtOption.Expiration });
            await _userManager.UpdateAsync(identityUser);

            return new TokenDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                Expiration = _jwtOption.Expiration
            };
        }
    }
}
