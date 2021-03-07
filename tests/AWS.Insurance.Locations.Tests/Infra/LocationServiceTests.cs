using AWS.Insurance.Locations.Domain.Errors;
using AWS.Insurance.Locations.Domain.Models;
using AWS.Insurance.Locations.Infra;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Insurance.Locations.Tests.Infra
{
    public class LocationServiceTests
    {
        [Fact(DisplayName = "Should return a zone")]
        public async Task GetZone_ShouldReturnAZone()
        {
            // ARRANGE
            var zipCode = 9999;

            // ACT
            var sut = new LocationService();
            var zone = await sut.GetZone(zipCode);

            // ASSERT
            Assert.NotNull(zone);
            Assert.IsType<Location>(zone);
        }

        [Fact(DisplayName = "Should return a null if zipCode greater than 10000")]
        public async Task GetZone_ShouldReturnANullIfZipCodeGreaterThan10000()
        {
            // ARRANGE
            var zipCode = 10001;

            // ACT
            var sut = new LocationService();

            // ASSERT
            await Assert.ThrowsAsync<RestException>(() => sut.GetZone(zipCode));
        }

        [Fact(DisplayName = "Should return a null if zipCode less or equal than zero")]
        public async Task GetZone_ShouldReturnANullIfZipCodeLessOrEqualZero()
        {
            // ARRANGE
            var zipCode = 0;

            // ACT
            var sut = new LocationService();

            // ASSERT
            await Assert.ThrowsAsync<RestException>(() => sut.GetZone(zipCode));
        }
    }
}
