using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace AWS.Insurance.Operations.Api.StartupExtensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection ConfigureSwaggerOperation(this IServiceCollection services)
        {
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Operations API",
                    Description = "Handle customers and cars",
                    Contact = new OpenApiContact
                    {
                        Name = "le.bevilaqua@gmail.com",
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opts.IncludeXmlComments(xmlPath);
                opts.CustomSchemaIds(x => x.FullName);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerOperation(this IApplicationBuilder builder)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Operations Api V1");
                c.RoutePrefix = "swagger";
            });

            return builder;
        }
    }
}