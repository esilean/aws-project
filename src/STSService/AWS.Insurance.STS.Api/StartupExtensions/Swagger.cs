using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace AWS.Insurance.STS.Api.StartupExtensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection ConfigureSwaggerIdentity(this IServiceCollection services)
        {
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Identity API",
                    Description = "Handle auth",
                    Contact = new OpenApiContact
                    {
                        Name = "le.bevilaqua@gmail.com",
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opts.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerIdentity(this IApplicationBuilder builder)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Api V1");
                c.RoutePrefix = "swagger";
            });

            return builder;
        }
    }
}
