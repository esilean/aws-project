using AWS.Insurance.Locations.Api.Controllers;
using AWS.Insurance.Locations.Domain.Interfaces;
using AWS.Insurance.Locations.Domain.Models;
using AWS.Insurance.Locations.Domain.Models.Enums;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Insurance.Locations.Tests.Api.UnitTests
{
    public class LocationsControllerTests
    {
        [Fact(DisplayName = "Should return a zone by zipCode")]
        public async Task Location_ShouldReturnZoneByZipCode()
        {
            // ARRANGE
            var zipCode = 9999;
            var mockLogger = Substitute.For<ILogger<LocationsController>>();
            var mockLocationService = Substitute.For<ILocationService>();

            var location = new Location(zipCode, Zone.Yellow);
            mockLocationService.GetZone(zipCode).Returns(location);

            // ACT
            var sut = new LocationsController(mockLogger, mockLocationService);
            var response = await sut.GetZone(zipCode);

            // ASSERT
            Assert.NotNull(response);
            Assert.Equal(location.ZipCode, response.ZipCode);
            Assert.Equal(location.Zone, response.Zone);
        }

        [Fact(DisplayName = "Should log a message")]
        public async Task Location_ShouldLogAMessage()
        {
            // ARRANGE
            var zipCode = 9999;
            var mockLogger = Substitute.For<ILogger<LocationsController>>();
            var mockLocationService = Substitute.For<ILocationService>();

            var location = new Location(zipCode, Zone.Yellow);
            mockLocationService.GetZone(zipCode).Returns(location);

            // ACT
            var sut = new LocationsController(mockLogger, mockLocationService);
            await sut.GetZone(zipCode);

            // ASSERT
            mockLogger.ReceivedWithAnyArgs(1).LogInformation("Looking for zone at ZipCode {zipCode}", zipCode);
        }
    }
}
