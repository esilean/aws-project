using AWS.Insurance.Operations.Application.Customers.Dtos;
using AWS.Insurance.Operations.Application.Errors;
using AWS.Insurance.Operations.Domain.Models.Enums;
using AWS.Insurance.Operations.Infra.Zones;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Insurance.Operations.Tests.Infra.Zones
{
    public class LocationServiceTests
    {
        [Fact(DisplayName = "Should return a location")]
        public async Task LocationService_ShouldReturnALocation()
        {
            // ARRANGE
            var locationDto = new LocationDto { ZipCode = 9999, Zone = Zone.Red };

            var mockLogger = Mock.Of<ILogger<LocationService>>();
            var mockFactory = new Mock<IHttpClientFactory>();

            var client = new HttpClient(MockHttpMessageHandler(HttpStatusCode.OK, locationDto).Object);
            client.BaseAddress = new Uri("http://localhost");

            mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            // ACT
            var sut = new LocationService(mockLogger, mockFactory.Object);
            var location = await sut.GetLocation(locationDto.ZipCode);

            // ASSERT
            Assert.Equal(locationDto.ZipCode, location.ZipCode);
            Assert.Equal(locationDto.Zone, location.Zone);
        }

        [Fact(DisplayName = "Should throw an error if zipCode is invalid")]
        public async Task LocationService_ShouldThrowAnErroZipCodeInvalid()
        {
            // ARRANGE
            var locationDto = new LocationDto { ZipCode = 9999, Zone = Zone.Red };

            var mockLogger = Mock.Of<ILogger<LocationService>>();
            var mockFactory = new Mock<IHttpClientFactory>();

            var client = new HttpClient(MockHttpMessageHandler(HttpStatusCode.NotFound, locationDto).Object);
            client.BaseAddress = new Uri("http://localhost");

            mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            // ACT
            var sut = new LocationService(mockLogger, mockFactory.Object);

            // ASSERT
            await Assert.ThrowsAsync<RestException>(() => sut.GetLocation(locationDto.ZipCode));
        }

        private Mock<HttpMessageHandler> MockHttpMessageHandler(HttpStatusCode stausCode, object returnsObject)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = stausCode,
                    Content = new StringContent(JsonConvert.SerializeObject(returnsObject)),
                });

            return mockHttpMessageHandler;
        }
    }
}
