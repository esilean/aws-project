using AWS.Insurance.STS.Api.StartupExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AWS.Insurance.STS.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog
                (
                    (hostingContext, loggerConfiguration) =>
                    {
                        loggerConfiguration
                            .AppendFileLogger("Logs/log-identity-.log")
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
