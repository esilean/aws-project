using AWS.Insurance.Operations.Data.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AWS.Insurance.Operations.Api.StartupExtensions
{
    public static class DBz
    {
        public static IServiceCollection ConfigureDatabaseConn(this IServiceCollection services,
                                                               IConfiguration configuration,
                                                               IWebHostEnvironment environment)
        {
            var connString = configuration.GetConnectionString("DefaultConnection");

            if (!environment.IsEnvironment("Testing"))
            {
                services.AddDbContext<OpDbContext>(opt =>
                {
                    opt.UseLazyLoadingProxies();
                    opt.UseMySql(connString).AddXRayInterceptor(true);
                });
            }

            return services;
        }
    }
}