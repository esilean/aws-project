using AWS.Insurance.Operations.Application.Customers;
using AWS.Insurance.Operations.Application.Customers.Dtos;
using AWS.Insurance.Operations.Application.Errors;
using AWS.Insurance.Operations.Application.Gateways;
using AWS.Insurance.Operations.Data.UoW;
using AWS.Insurance.Operations.Domain.Models;
using AWS.Insurance.Operations.Domain.Models.Enums;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Insurance.Operations.Tests.Application.Customers
{
    public class CreateTests
    {
        [Fact(DisplayName = "Should validate a customer create command")]
        public void Customer_ValidateCustomerCreateCommand()
        {
            // ARRANGE
            var sut = new Create.Command
            {
                Id = Guid.NewGuid(),
                Name = "Bevila",
                Age = 33,
                CNumber = 1234,
                Dob = DateTime.Today,
                ZipCode = 9999
            };

            // ACT
            var validator = new Create.CommandValidator();
            var result = validator.Validate(sut);

            // ASSERT
            Assert.True(result.IsValid);
        }

        [Theory(DisplayName = "Should not validate a customer create command if some prop is invalid")]
        [InlineData("", 33, 1234, "1900-01-01", 9999)]
        [InlineData("Bevila", 0, 1234, "1900-01-01", 9999)]
        [InlineData("Bevila", 33, 0, "1900-01-01", 9999)]
        [InlineData("Bevila", 33, 1234, "1900-01-01", 0)]
        public void Customer_NotValidateCustomerCreateCommand(string name, int age, int cNumber, DateTime dob, int zipCode)
        {
            // ARRANGE
            var sut = new Create.Command
            {
                Id = Guid.NewGuid(),
                Name = name,
                Age = age,
                CNumber = cNumber,
                Dob = dob,
                ZipCode = zipCode
            };

            // ACT
            var validator = new Create.CommandValidator();
            var result = validator.Validate(sut);

            // ASSERT
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Should throw a RestException if Cnumber is already in use")]
        public async Task Customer_ThrowARestExceptionIfCNumberIsInUse()
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
            new Create.CommandValidator().Validate(command);

            var mockCache = new Mock<IDistributedCache>();
            var mockUOW = new Mock<IUnitOfWork>();
            mockUOW.Setup(x => x.Customers.GetByCNumberAsync(command.CNumber))
                            .Returns(Task.FromResult(new Customer(command.CNumber, command.Name, command.Age, command.Dob, command.ZipCode)));

            var mockLocationService = new Mock<ILocationService>();


            // ACT
            var sut = new Create.Handler(mockUOW.Object, mockCache.Object, mockLocationService.Object);

            // ASSERT
            await Assert.ThrowsAsync<RestException>(() => sut.Handle(command, CancellationToken.None));

        }

        [Fact(DisplayName = "Should handle a valid a customer create command")]
        public async Task Customer_HandleAValidCustomerCreateCommand()
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
            new Create.CommandValidator().Validate(command);

            var mockCache = new Mock<IDistributedCache>();
            var mockUOW = new Mock<IUnitOfWork>();
            mockUOW.Setup(x => x.Customers.GetByCNumberAsync(It.IsAny<int>()));
            mockUOW.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true));

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(x => x.GetLocation(It.IsAny<int>()))
                    .Returns(Task.FromResult(new LocationDto
                    {
                        ZipCode = command.ZipCode,
                        Zone = Zone.Yellow
                    }));

            // ACT
            var sut = new Create.Handler(mockUOW.Object, mockCache.Object, mockLocationService.Object);
            var result = await sut.Handle(command, CancellationToken.None);

            // ASSERT
            mockUOW.Verify(x => x.Customers.AddAsync(It.IsAny<Customer>()), Times.Once);
            mockUOW.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact(DisplayName = "Should throw a Exception if problem saving changes")]
        public async Task Customer_ThrowAExceptionIfProblemSavingChanges()
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
            new Create.CommandValidator().Validate(command);

            var mockCache = new Mock<IDistributedCache>();
            var mockUOW = new Mock<IUnitOfWork>();
            mockUOW.Setup(x => x.Customers.GetByCNumberAsync(It.IsAny<int>()));
            mockUOW.Setup(x => x.SaveAsync()).Returns(Task.FromResult(false));

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService.Setup(x => x.GetLocation(It.IsAny<int>()))
                    .Returns(Task.FromResult(new LocationDto
                    {
                        ZipCode = command.ZipCode,
                        Zone = Zone.Yellow
                    }));

            // ACT
            var sut = new Create.Handler(mockUOW.Object, mockCache.Object, mockLocationService.Object);

            // ASSERT
            await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));
        }


    }
}
