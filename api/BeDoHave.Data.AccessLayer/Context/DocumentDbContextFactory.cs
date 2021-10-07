using BeDoHave.Shared.Factories;
using Microsoft.EntityFrameworkCore;

namespace BeDoHave.Data.AccessLayer.Context
{
    class DocumentDbContextFactory : DesignTimeDbContextFactoryBase<DocumentDbContext>
    {
        protected override DocumentDbContext CreateNewInstance(DbContextOptions<DocumentDbContext> options)
        {
            return new DocumentDbContext(options);
        }
    }
}
