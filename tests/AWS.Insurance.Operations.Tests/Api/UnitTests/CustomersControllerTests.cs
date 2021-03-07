using AWS.Insurance.Operations.Api.Controllers;
using AWS.Insurance.Operations.Application.Customers;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Insurance.Operations.Tests.Api.UnitTests
{
    public class CustomersControllerTests
    {

        [Fact(DisplayName = "Should create a customer")]
        public async Task Customer_ShouldCreateACustomer()
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

            var mockLogger = new Mock<ILogger<CustomersController>>();
            var mockIMediator = new Mock<IMediator>();

            // ACT
            var sut = new CustomersController(mockLogger.Object, mockIMediator.Object);
            var response = await sut.Create(command);

            // ASSERT
            mockIMediator.Verify(x => x.Send(command, CancellationToken.None));
        }
    }
}
