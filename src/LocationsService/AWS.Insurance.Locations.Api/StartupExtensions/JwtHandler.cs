using AWS.Insurance.Operations.Infra.Security.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace AWS.Insurance.Locations.Api.StartupExtensions
{
    public static class JwtHandler
    {
        public static IServiceCollection ConfigureJwt(this IServiceCollection services,
                                                           IConfiguration configuration,
                                                           IWebHostEnvironment environment)
        {
            if (!environment.IsEnvironment("Testing"))
            {
                services.Configure<AppCredentialsSettings>(configuration.GetSection("AppCredentials"));
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JwtSettings").GetValue<string>("SigningKey")));
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = key,
                        ValidateIssuerSigningKey = true,
                        ValidAudiences = new List<string> { "http://127.0.0.1" },
                        ValidateAudience = true,
                        ValidIssuers = new List<string> { "http://127.0.0.1" },
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            }

            return services;
        }
    }
}
