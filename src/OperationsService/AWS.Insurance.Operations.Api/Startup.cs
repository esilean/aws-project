using System;
using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using AWS.Insurance.Operations.Api.Middlewares;
using AWS.Insurance.Operations.Api.StartupExtensions;
using AWS.Insurance.Operations.Application.Cars;
using AWS.Insurance.Operations.Data.Context;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace AWS.Insurance.Operations.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(cfg =>
                {
                    cfg.RegisterValidatorsFromAssemblyContaining<Create>();
                });

            services
                    .ConfigureDatabaseConn(Configuration, HostingEnvironment)
                    .ConfigureHttpClients(Configuration)
                    .ConfigureHttpContextAcessor()
                    .ConfigureDistributedCached(Configuration)
                    .ConfigureIOC(Configuration)
                    .ConfigureHealthzCheck(Configuration)
                    .ConfigureSwaggerOperation()
                    .ConfigureAWS(Configuration, HostingEnvironment);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, OpDbContext context, ILogger<Startup> logger)
        {
            Log.Information($"Hosting enviroment = {env.EnvironmentName}");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAWS();
            app.UseSwaggerOperation();
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseErrorHandlerMiddleware();
            app.UseEndpointz(env);

            Seed(context, logger);
        }

        private void Seed(OpDbContext context, ILogger<Startup> logger)
        {
            // XRAY - EFCore - AsyncLocal Problems
            AWSXRayRecorder.Instance.BeginSegment("DB Migration"); // generates `TraceId` for you
            try
            {
                logger.LogInformation("Initializing Database Migration.");
                context.Database.Migrate();
                logger.LogInformation("Finishing Database Migration...");
            }
            catch (Exception e)
            {
                AWSXRayRecorder.Instance.AddException(e);
            }
            finally
            {
                AWSXRayRecorder.Instance.EndSegment();
            }
        }
    }
}
