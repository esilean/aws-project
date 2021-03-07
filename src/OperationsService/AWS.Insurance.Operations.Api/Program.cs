using AWS.Insurance.Operations.Api.StartupExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace AWS.Insurance.Operations.Api
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog
                (
                    (hostingContext, loggerConfiguration) =>
                    {
                        loggerConfiguration
                            .AppendFileLogger("Logs/log-operation-.log")
                            .AppendConsoleLogger()
                            .AppendAwsCloudwatchLogger("index-aws-project", hostingContext.HostingEnvironment.EnvironmentName, Serilog.Events.LogEventLevel.Information)
                            .Enrich.FromLogContext()
                            .Enrich.WithMachineName()
                            .MinimumLevel.Information()
                            .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning);
                    }
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseIISIntegration();
                    webBuilder.UseKestrel(x => x.AddServerHeader = false);
                    webBuilder.UseStartup<Startup>();
                });
    }
}
