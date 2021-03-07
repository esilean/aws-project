using Amazon.XRay.Recorder.Handlers.System.Net;
using AutoMapper;
using AWS.Insurance.Operations.Application.Cars;
using AWS.Insurance.Operations.Application.Gateways;
using AWS.Insurance.Operations.Data.Context;
using AWS.Insurance.Operations.Data.UoW;
using AWS.Insurance.Operations.Infra.Security;
using AWS.Insurance.Operations.Infra.Security.Configs;
using AWS.Insurance.Operations.Infra.Zones;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AWS.Insurance.Operations.Api.StartupExtensions
{
    public static class IoC
    {
        public static IServiceCollection ConfigureIOC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(Create.Handler).Assembly);
            services.AddAutoMapper(typeof(Create.Handler).Assembly);

            services.AddScoped<ILocationService, LocationService>()
                                .AddHttpClient<HttpClientXRayTracingHandler>();

            services.AddScoped<IAppCredentialsService, AppCredentialsService>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<OpDbContext>();

            services.Configure<AppCredentialsSettings>(configuration.GetSection("AppCredentials"));

            return services;
        }
    }
}