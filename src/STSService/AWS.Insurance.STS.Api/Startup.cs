using Amazon.XRay.Recorder.Core;
using Amazon.XRay.Recorder.Handlers.AwsSdk;
using AWS.Insurance.STS.Api.Middlewares;
using AWS.Insurance.STS.Api.StartupExtensions;
using AWS.Insurance.STS.Application.App;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AWS.Insurance.STS.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

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
                    cfg.RegisterValidatorsFromAssemblyContaining<Auth>();
                });

            services
                    .ConfigureIOC(Configuration)
                    .ConfigureSwaggerIdentity()
                    .ConfigureAWS(Configuration, HostingEnvironment);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseSwaggerIdentity();
            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseErrorHandlerMiddleware();
            app.UseEndpointz(env);
        }
    }
}
