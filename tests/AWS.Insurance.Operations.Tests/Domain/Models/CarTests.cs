using AWS.Insurance.Operations.Domain.Errors;
using AWS.Insurance.Operations.Domain.Models;
using AWS.Insurance.Operations.Domain.Models.Enums;
using Moq;
using System;
using Xunit;

namespace AWS.Insurance.Operations.Tests.Domain.Models
{
    public class CarTests
    {
        [Fact(DisplayName = "Should create a car")]
        public void Customer_ShouldCreateACar()
        {
            // ARRANGE
            var customerId = Guid.NewGuid();
            var branchType = BranchType.BMW;
            var name = "ZZ";
            var year = 2020;

            // ACT
            var car = new Car(customerId, branchType, name, year);

            // ASSERT
            Assert.Equal(customerId, car.CustomerId);
            Assert.Equal(branchType, car.BranchType);
            Assert.Equal(name, car.Name);
            Assert.Equal(year, car.Year);
        }

        [Fact(DisplayName = "Should not create a car if custoemr is empty")]
        public void Customer_ShouldNotCreateCarIfCustomerIsEmpty()
        {
            // ARRANGE
            var customerId = Guid.Empty;

            // ACT
            // ASSERT
            Assert.Throws<DomainException>(() => new Car(customerId, It.IsAny<BranchType>(), It.IsAny<string>(), It.IsAny<int>()));
        }
    }
}
