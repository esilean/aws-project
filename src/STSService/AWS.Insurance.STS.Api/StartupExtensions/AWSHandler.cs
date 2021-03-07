using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AWS.Insurance.STS.Api.StartupExtensions
{
    public static class AWSHandler
    {
        public static IServiceCollection ConfigureAWS(this IServiceCollection services,
                                                           IConfiguration configuration,
                                                           IWebHostEnvironment environment)
        {
            if (!environment.IsEnvironment("Testing"))
            {
                AWSXRayRecorder recorder = new AWSXRayRecorderBuilder().Build();
                AWSXRayRecorder.InitializeInstance(configuration, recorder);
                AWSSDKHandler.RegisterXRayForAllServices();
            }

            return services;
        }

        public static IApplicationBuilder UseAWS(this IApplicationBuilder builder)
        {
            builder.UseXRay("AuthApi");

            return builder;
        }
    }
}
