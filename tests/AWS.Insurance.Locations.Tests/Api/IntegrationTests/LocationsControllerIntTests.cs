using AWS.Insurance.Locations.Api;
using AWS.Insurance.Locations.Domain.Models;
using AWS.Insurance.Locations.Domain.Models.Enums;
using AWS.Insurance.Locations.Tests.Api.AConfig;
using AWS.Insurance.Locations.Tests.Api.IntegrationTests.Data;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Insurance.Locations.Tests.Api.IntegrationTests
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class LocationsControllerIntTests
    {
        private readonly IntegrationTestsFixture<Startup> _testsFixture;

        public LocationsControllerIntTests(IntegrationTestsFixture<Startup> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Should return a success 200 Status")]
        public async Task WhenZipCodeIsValid_ShouldReturnA200StatusCode()
        {
            // ARRANGE
            var zipCode = 9999;

            // ACT
            //_testsFixture.Client.AddToken(_testsFixture.Token);
            var response = await _testsFixture.Client.GetAsync($"/api/locations/{zipCode}");

            // ASSERT
            response.EnsureSuccessStatusCode();
        }

        [Theory(DisplayName = "Should return a Location Object")]
        [ClassData(typeof(ZipCodeDataTest))]
        public async Task WhenZipCodeIsValid_ShouldReturnALocationObject(int zipCode)
        {
            // ARRANGE
            // ACT
            //_testsFixture.Client.AddToken(_testsFixture.Token);
            var response = await _testsFixture.Client.GetAsync($"/api/locations/{zipCode}");
            var result = DeserializeLocation(await response.Content.ReadAsStringAsync());

            // ASSERT
            Assert.Equal(zipCode, result.ZipCode);
            Assert.IsType<Zone>(result.Zone);
        }

        [Fact(DisplayName = "Should return a not found 404 Status")]
        public async Task WhenZipCodeIsInValid_ShouldReturnA404NotFound()
        {
            // ARRANGE
            var zipCode = 0;

            // ACT
            //_testsFixture.Client.AddToken(_testsFixture.Token);
            var response = await _testsFixture.Client.GetAsync($"/api/locations/{zipCode}");

            // ASSERT
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(404, (int)response.StatusCode);
        }

        private Location DeserializeLocation(string data)
        {
            return JsonConvert.DeserializeObject<Location>(data);
        }
    }
}
