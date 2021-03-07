using AWS.Insurance.Operations.Api.Controllers;
using AWS.Insurance.Operations.Application.Cars;
using AWS.Insurance.Operations.Domain.Models.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Insurance.Operations.Tests.Api.UnitTests
{
    public class CarsControllerTests
    {

        [Fact(DisplayName = "Should create a customer")]
        public async Task Customer_ShouldCreateACustomer()
        {
            // ARRANGE
            var command = new Create.Command
            {
                CustomerId = Guid.NewGuid(),
                BranchType = (int)BranchType.BMW,
                Name = "ZZ",
                Year = 2000
            };

            var mockLogger = new Mock<ILogger<CarsController>>();
            var mockIMediator = new Mock<IMediator>();

            // ACT
            var sut = new CarsController(mockLogger.Object, mockIMediator.Object);
            var response = await sut.Create(command);

            // ASSERT
            mockIMediator.Verify(x => x.Send(command, CancellationToken.None));
        }
    }
}
