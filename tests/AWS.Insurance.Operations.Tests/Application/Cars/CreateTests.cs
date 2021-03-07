using AWS.Insurance.Operations.Application.Cars;
using AWS.Insurance.Operations.Application.Errors;
using AWS.Insurance.Operations.Data.UoW;
using AWS.Insurance.Operations.Domain.Models;
using AWS.Insurance.Operations.Domain.Models.Enums;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Insurance.Operations.Tests.Application.Cars
{
    public class CreateTests
    {
        [Fact(DisplayName = "Should validate a car create command")]
        public void Car_ValidateCustomerCreateCommand()
        {
            // ARRANGE
            var sut = new Create.Command
            {
                CustomerId = Guid.NewGuid(),
                BranchType = (int)BranchType.BMW,
                Name = "ZZ",
                Year = 2000
            };

            // ACT
            var validator = new Create.CommandValidator();
            var result = validator.Validate(sut);

            // ASSERT
            Assert.True(result.IsValid);
        }

        [Theory(DisplayName = "Should not validate a car create command if some prop is invalid")]
        [InlineData(0, "ZZ", 1900)]
        [InlineData(1, "", 1900)]
        [InlineData(1, "ZZ", 0)]
        public void Car_NotValidateCustomerCreateCommand(int branchType, string name, int year)
        {
            // ARRANGE
            var sut = new Create.Command
            {
                CustomerId = Guid.NewGuid(),
                BranchType = branchType,
                Name = name,
                Year = year
            };

            // ACT
            var validator = new Create.CommandValidator();
            var result = validator.Validate(sut);

            // ASSERT
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Should throw a RestException if Customer does not exist")]
        public async Task Car_ThrowARestExceptionIfCustomerDoesNotExist()
        {
            // ARRANGE
            var customer = new Customer(1324, "Bevila", 33, DateTime.Today, 9999);
            var command = new Create.Command
            {
                CustomerId = customer.Id,
                BranchType = (int)BranchType.BMW,
                Name = "ZZ",
                Year = 1900
            };
            new Create.CommandValidator().Validate(command);

            var mockUOW = new Mock<IUnitOfWork>();
            mockUOW.Setup(x => x.Customers.GetByIdAsync(customer.Id));

            // ACT
            var sut = new Create.Handler(mockUOW.Object);

            // ASSERT
            await Assert.ThrowsAsync<RestException>(() => sut.Handle(command, CancellationToken.None));

        }

        [Fact(DisplayName = "Should handle a valid a car create command")]
        public async Task Car_HandleAValidCustomerCreateCommand()
        {
            // ARRANGE
            var customer = new Customer(1324, "Bevila", 33, DateTime.Today, 9999);
            var command = new Create.Command
            {
                CustomerId = customer.Id,
                BranchType = (int)BranchType.BMW,
                Name = "ZZ",
                Year = 1900
            };
            new Create.CommandValidator().Validate(command);

            var mockUOW = new Mock<IUnitOfWork>();
            mockUOW.Setup(x => x.Customers.GetByIdAsync(customer.Id))
                            .Returns(Task.FromResult(customer));
            mockUOW.Setup(x => x.Cars.AddAsync(It.IsAny<Car>()));
            mockUOW.Setup(x => x.SaveAsync()).Returns(Task.FromResult(true));

            // ACT
            var sut = new Create.Handler(mockUOW.Object);
            await sut.Handle(command, CancellationToken.None);

            // ASSERT
            mockUOW.Verify(x => x.Cars.AddAsync(It.IsAny<Car>()), Times.Once);
            mockUOW.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact(DisplayName = "Should throw a Exception if problem saving changes")]
        public async Task Car_ThrowAExceptionIfProblemSavingChanges()
        {
            // ARRANGE
            var customer = new Customer(1324, "Bevila", 33, DateTime.Today, 9999);
            var command = new Create.Command
            {
                CustomerId = customer.Id,
                BranchType = (int)BranchType.BMW,
                Name = "ZZ",
                Year = 1900
            };
            new Create.CommandValidator().Validate(command);

            var mockUOW = new Mock<IUnitOfWork>();
            mockUOW.Setup(x => x.Customers.GetByIdAsync(customer.Id))
                            .Returns(Task.FromResult(customer));
            mockUOW.Setup(x => x.Cars.AddAsync(It.IsAny<Car>()));
            mockUOW.Setup(x => x.SaveAsync()).Returns(Task.FromResult(false));

            // ACT
            var sut = new Create.Handler(mockUOW.Object);

            // ASSERT
            await Assert.ThrowsAsync<Exception>(() => sut.Handle(command, CancellationToken.None));
        }


    }
}
