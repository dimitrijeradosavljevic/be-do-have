using BeDoHave.Application.Document.Interfaces;
using BeDoHave.Application.Document.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BeDoHave.Application.Document.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IOrganisationService, OrganisationService>();
            services.AddScoped<IDocumentService, DocumentService>();

            return services;
        }
    }
}
