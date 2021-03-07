using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AWS.Insurance.Operations.Api.StartupExtensions
{
    public static class Healthz
    {
        public static IServiceCollection ConfigureHealthzCheck(this IServiceCollection services,
                                                                    IConfiguration configuration)
        {
            var uriServiceName = configuration.GetSection("UriServiceName").Value;

            services.AddHealthChecks()
                    .AddMySql(
                        connectionString: configuration.GetConnectionString("DefaultConnection"),
                        name: "MySql Server DB",
                        timeout: TimeSpan.FromSeconds(60),
                        tags: new string[] { "ready" },
                        failureStatus: HealthStatus.Unhealthy);

            return services;
        }
    }
}