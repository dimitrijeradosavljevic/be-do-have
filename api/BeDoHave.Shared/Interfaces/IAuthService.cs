using BeDoHave.Shared.Dtos;
using System.Threading.Tasks;

namespace BeDoHave.Shared.Interfaces
{
    public interface IAuthService
    {
        Task<string> CreateIdentityAsync(RegisterDto registerDto);
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<TokenDto> RevokeAsync(RevokeTokenDto revokeTokenDto);
        Task DeleteIdentityAsync(string identityUserId);
        Task<bool> IsAdmin(string identityUserId);
        Task<string> FindIdByNameAsync(string userName);
        Task<string> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
