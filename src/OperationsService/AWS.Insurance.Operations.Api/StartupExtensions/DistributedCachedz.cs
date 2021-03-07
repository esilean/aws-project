using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AWS.Insurance.Operations.Api.StartupExtensions
{
    public static class DistributedCachedz
    {
        public static IServiceCollection ConfigureDistributedCached(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConn = configuration.GetConnectionString("RedisConnection");
            if (string.IsNullOrWhiteSpace(redisConn))
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConn;
                    options.InstanceName = "Insurance:";
                });
            }

            return services;
        }

    }
}
