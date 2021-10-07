using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BeDoHave.Identity.Cache.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider()
                .GetService<IConfiguration>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisConnection");
            });

            services.AddSingleton<IDistributedCache, RedisCache>();

            return services;
        }
    }
}
