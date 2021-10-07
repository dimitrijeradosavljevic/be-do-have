using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace BeDoHave.Identity.Models
{
    public class IdentityAppUser : IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
