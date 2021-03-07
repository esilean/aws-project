using AWS.Insurance.Operations.Api;
using AWS.Insurance.Operations.Application.Customers;
using AWS.Insurance.Operations.Tests.Api.AConfig;
using AWS.Insurance.Operations.Tests.Api.AConfig.Dtos;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Insurance.Locations.Tests.Api.IntegrationTests
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class CustomersControllerIntTests
    {
        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public CustomersControllerIntTests(IntegrationTestsFixture<Startup> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Should return a success 200 Status")]
        public async Task Customer_WhenCommandValid_ShouldReturnA200StatusCode()
        {
            // ARRANGE
            var command = new Create.Command
            {
                Id = Guid.NewGuid(),
                Name = "Bevila",
                Age = 33,
                CNumber = 9999,
                Dob = DateTime.Today,
                ZipCode = 9999
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // ACT
            var response = await _testsFixture.Client.PostAsync($"/api/customers", stringContent);

            // ASSERT
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Should return an error 400 Status if Cnumber already exists")]
        public async Task Customer_WhenCnumberExists_ShouldReturnA400StatusCode()
        {
            // ARRANGE
            var command = new Create.Command
            {
                Id = Guid.NewGuid(),
                Name = "Bevila",
                Age = 33,
                CNumber = 1234,
                Dob = DateTime.Today,
                ZipCode = 9999
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            await _testsFixture.Client.PostAsync($"/api/customers", stringContent);

            // ACT
            var response = await _testsFixture.Client.PostAsync($"/api/customers", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ErrorDto>(result);

            // ASSERT
            Assert.Equal(400, (int)response.StatusCode);
            Assert.Equal("Cnumber already taken.", data.Errors.Message);
        }



    }
}
