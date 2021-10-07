using Microsoft.EntityFrameworkCore;
using BeDoHave.Shared.Factories;

namespace BeDoHave.Identity.Contexts
{
    public class AppDbContextFactory : DesignTimeDbContextFactoryBase<IdentityAppDbContext>
    {
        protected override IdentityAppDbContext CreateNewInstance(DbContextOptions<IdentityAppDbContext> options)
        {
            return new IdentityAppDbContext(options);
        }
    }
}
