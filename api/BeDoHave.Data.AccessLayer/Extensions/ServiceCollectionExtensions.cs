using BeDoHave.Data.AccessLayer.Context;
using BeDoHave.Data.AccessLayer.Interfaces;
using BeDoHave.Data.AccessLayer.Repositories;
using BeDoHave.Data.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeDoHave.Data.AccessLayer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider()
                .GetService<IConfiguration>();

            services.AddDbContext<DocumentDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DocumentDatabaseConnection"),
                    assembly => assembly.MigrationsAssembly(typeof(DocumentDbContext).Assembly.FullName));
            });

            services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));

            services.AddScoped<IOrganisationProcedureRepository, OrganisationProcedureRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IOrganisationRepository, OrganisationRepository>();
            services.AddScoped<IOrganisationInviteRepository, OrganisationInviteRepository>();
            services.AddScoped<ITagRepository, TagRepository>();

            return services;
        }
    }
}
