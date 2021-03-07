using AWS.Insurance.STS.Application.App;
using AWS.Insurance.STS.Application.Gateways;
using AWS.Insurance.STS.Infra.AppAcessor;
using AWS.Insurance.STS.Infra.Security;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AWS.Insurance.STS.Api.StartupExtensions
{
    public static class IoC
    {
        public static IServiceCollection ConfigureIOC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(Auth.Handler).Assembly);

            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IAppAccessor, AppAcessorFake>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<AppAccessorSettings>(configuration.GetSection("AppAccessorSettings"));

            return services;
        }
    }
}
