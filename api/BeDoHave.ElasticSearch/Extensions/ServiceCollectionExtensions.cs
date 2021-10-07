using BeDoHave.ElasticSearch.Entities;
using BeDoHave.ElasticSearch.Interfaces;
using BeDoHave.ElasticSearch.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Page = BeDoHave.Data.Core.Entities.Page;

namespace BeDoHave.ElasticSearch.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services)
        {
            var settings = new ConnectionSettings().EnableDebugMode();
                // .DefaultMappingFor<PageSearch>(page => page.IndexName("pages"));
            
            
                
            var client = new ElasticClient(settings);

            // client.Indices.Delete("pages");
            // client.Indices.Create("pages", c => c
            //     .Map<PageSearch>(m => m
            //         .Properties(p =>
            //             p.Completion(c => c.Name(p => p.Suggest)))));
            

            services.AddSingleton<IElasticClient>(new ElasticClient(settings));

            services.AddScoped<ISearchPageRepository, SearchPageRepository>();
            
            return services;
        }
    }
}