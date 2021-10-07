using System.Collections.Generic;
using System.Security.Claims;

namespace BeDoHave.Shared.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(string username, string userId, IList<string> role);
        string GenerateRefreshToken();
        bool InvalidateOrCheckAccessToken(string token, bool check = false);
        ClaimsPrincipal GetPrincipalFromToken(string token);
        Claim GetUsernameClaimFromToken(string token);
        (string Email, string IdentityId) GetUserClaimsFromToken(string token);
    }
}
