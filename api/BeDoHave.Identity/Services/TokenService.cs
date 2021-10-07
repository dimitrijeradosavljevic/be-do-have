using BeDoHave.Identity.Options;
using BeDoHave.Shared.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BeDoHave.Identity.Services
{
    public class TokenService : ITokenService
    {
        private readonly IDistributedCache _cache;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly JwtOption _jwtOptions;

        public TokenService(
            IOptions<JwtOption> jwtOption,
            JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _jwtOptions = jwtOption.Value;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }


        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public string GenerateAccessToken(string username, string userId, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.Jti),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            return _jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
        }

        public bool InvalidateOrCheckAccessToken(string token, bool check = false)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            var principals = GetPrincipalFromToken(token);
            var jtiClaim = principals?.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
            var expClaim = principals?.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

            if (jtiClaim is null || expClaim is null)
            {
                return false;
            }

            if (!double.TryParse(expClaim.Value, out double tokenExpirationTimestamp))
            {
                return false;
            }

            var memoryCacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow + GetExpirationFromTimestamp(tokenExpirationTimestamp)
            };


            return true;
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            token = token.Replace("Bearer ", string.Empty);
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SigningKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = _jwtOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            var claimsPrincipals =
                _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || (jwtSecurityToken.Issuer != _jwtOptions.Issuer)
                || (jwtSecurityToken.ValidTo < DateTime.UtcNow))
            {
                throw new SecurityTokenException("The token is not valid.");
            }

            return claimsPrincipals;
        }

        public Claim GetUsernameClaimFromToken(string token)
        {
            token = token.Replace("Bearer ", string.Empty);

            var jwtToken = _jwtSecurityTokenHandler.ReadJwtToken(token);

            return jwtToken.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Name);
        }

        public (string Email, string IdentityId) GetUserClaimsFromToken(string token)
        {
            token = token.Replace("Bearer ", string.Empty);

            var jwtToken = _jwtSecurityTokenHandler.ReadJwtToken(token);

            var identityId = jwtToken.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
            var username = jwtToken.Claims.First(c => c.Type == ClaimTypes.Name);

            return new(username.Value, identityId.Value);
        }

        private static TimeSpan GetExpirationFromTimestamp(double unixTimestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0)
                .AddSeconds(unixTimestamp)
                .Subtract(DateTime.UtcNow);
        }
    }
}