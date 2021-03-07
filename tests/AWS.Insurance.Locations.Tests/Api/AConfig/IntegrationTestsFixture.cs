using AWS.Insurance.Locations.Api;
using AWS.Insurance.Locations.Tests.Api.AConfig.Mock;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using Xunit;

namespace AWS.Insurance.Locations.Tests.Api.AConfig
{
    [CollectionDefinition(nameof(IntegrationTestsFixtureCollection))]
    public class IntegrationTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Startup>>
    { }
    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly AppFactory<TStartup> Factory;
        public HttpClient Client;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true
            };

            Factory = new AppFactory<TStartup>();
            Client = Factory.CreateClient(clientOptions);

            var token = GenerateToken();
            Client.AddToken(token);
        }

        private string GenerateToken()
        {
            var jwtGenerator = new JwtGeneratorMock(new JwtSettings
            {
                SigningKey = "123kzfSPDKwdx5KnyxtBTlwNW_IoqrpbaGRwaFNdqxQyv-WVIqeLKOGJVLmh4"
            });

            return jwtGenerator.CreateToken();
        }

        public void Dispose()
        {
            Factory.Dispose();
            Client.Dispose();
        }
    }
}