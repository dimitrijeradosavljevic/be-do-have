using BeDoHave.Application.Interfaces;
using BeDoHave.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BeDoHave.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IOrganisationService, OrganisationService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrganisationInviteService, OrganisationInviteService>();
            services.AddScoped<ITagService, TagService>();

            return services;
        }
    }
}
