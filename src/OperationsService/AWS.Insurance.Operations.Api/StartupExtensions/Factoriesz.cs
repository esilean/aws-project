using Amazon.XRay.Recorder.Handlers.System.Net;
using AWS.Insurance.Operations.Api.StartupExtensions.HttpMessageHandler;
using AWS.Insurance.Operations.Application.Gateways;
using AWS.Insurance.Operations.Infra.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace AWS.Insurance.Operations.Api.StartupExtensions
{
    public static class Factoriesz
    {
        public static IServiceCollection ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var locationService = configuration.GetSection("ExternalAPIs:LocationApi").Value;

            // # Token
            services.AddHttpClient<IAppCredentialsService, AppCredentialsService>();

            // # Location Service
            services.AddTransient<TokenMessageHandler>();
            services.AddHttpClient("LocationService", client =>
            {
                client.BaseAddress = new Uri($"{locationService}/");
                client.Timeout = TimeSpan.FromSeconds(20);
            })
            .AddHttpMessageHandler<TokenMessageHandler>()
            .ConfigurePrimaryHttpMessageHandler(() =>
             {
                 return new HttpClientXRayTracingHandler(new HttpClientHandler());
             });

            return services;
        }
    }
}