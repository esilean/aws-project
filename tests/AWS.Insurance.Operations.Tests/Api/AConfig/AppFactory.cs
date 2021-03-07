using AWS.Insurance.Operations.Application.Gateways;
using AWS.Insurance.Operations.Data.Context;
using AWS.Insurance.Operations.Tests.Api.AConfig.Stubs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AWS.Insurance.Operations.Tests.Api.AConfig
{
    public class AppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.AddDbContext<OpDbContext>(opts =>
                {
                    opts.UseInMemoryDatabase("OpInMemory");
                });

                services.AddScoped<ILocationService, LocationServiceStub>();
            });
        }
    }
}
