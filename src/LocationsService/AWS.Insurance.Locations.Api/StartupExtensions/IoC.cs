using AWS.Insurance.Locations.Domain.Interfaces;
using AWS.Insurance.Locations.Infra;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace AWS.Insurance.Locations.Api.StartupExtensions
{
    public static class IoC
    {
        public static IServiceCollection ConfigureIOC(this IServiceCollection services)
        {
            services.AddScoped<ILocationService, LocationService>();

            return services;
        }
    }
}