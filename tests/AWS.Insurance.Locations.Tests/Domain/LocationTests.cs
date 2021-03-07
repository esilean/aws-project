using AWS.Insurance.Locations.Domain.Errors;
using AWS.Insurance.Locations.Domain.Models;
using AWS.Insurance.Locations.Domain.Models.Enums;
using Xunit;

namespace AWS.Insurance.Locations.Tests.Domain
{
    public class LocationTests
    {
        [Fact(DisplayName = "Should create an Location")]
        public void Location_ShouldCreateObjectLocation()
        {
            // ARRANGE
            var zipCode = 10000;
            var zone = Zone.Green;

            // ACT
            var location = new Location(zipCode, zone);

            // ASSERT
            Assert.Equal(zipCode, location.ZipCode);
            Assert.Equal(zone, location.Zone);
        }

        [Fact(DisplayName = "Should not create Location if ZipCode less or equal to zero")]
        public void Location_ShouldNotCreateObjectLocationIfZipCodeLessEqualToZero()
        {
            // ARRANGE
            var zipCode = 0;
            var zone = Zone.Green;

            // ACT
            // ASSERT
            Assert.Throws<DomainException>(() => new Location(zipCode, zone));
        }

        [Theory(DisplayName = "Should not create an object of type Location if ZipCode greater than 10000")]
        [InlineData(10001)]
        [InlineData(10002)]
        [InlineData(99999)]
        public void Location_ShouldNotCreateObjectLocationIfZipCodeGreaterThan10000(int zipCode)
        {
            // ARRANGE
            var zone = Zone.Green;

            // ACT
            // ASSERT
            Assert.Throws<DomainException>(() => new Location(zipCode, zone));
        }
    }
}
