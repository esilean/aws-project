using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace AWS.Insurance.Locations.Api.StartupExtensions
{
    public static class EndPointz
    {
        public static IApplicationBuilder UseEndpointz(this IApplicationBuilder builder, IWebHostEnvironment env)
        {
            builder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                });

                endpoints.Map("/", async (context) =>
                {
                    var result = JsonSerializer.Serialize(new
                    {
                        machineName = Environment.MachineName,
                        appName = env.ApplicationName,
                        environment = env.EnvironmentName
                    });

                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(result.ToString());
                });
            });
            return builder;
        }
    }
}