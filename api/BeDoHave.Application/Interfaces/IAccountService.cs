using BeDoHave.Application.Dtos;
using BeDoHave.Shared.Dtos;
using System.Threading.Tasks;

namespace BeDoHave.Application.Interfaces
{
    public interface IAccountService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<TokenDto> RegisterAsync(RegisterDto registerDto);
        Task<TokenDto> RevokeTokenAsync(RevokeTokenDto revokeTokenDto);
        Task<string> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<UserWithEmailDto> GetAuthenticatedUserAsync(string token);
    }
}
