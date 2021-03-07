using AWS.Insurance.Operations.Domain.Errors;
using AWS.Insurance.Operations.Domain.Models;
using AWS.Insurance.Operations.Domain.Models.Enums;
using Moq;
using System;
using Xunit;

namespace AWS.Insurance.Operations.Tests.Domain.Models
{
    public class CustomerTests
    {
        [Fact(DisplayName = "Should create a customer")]
        public void Customer_ShouldCreateACustomer()
        {
            // ARRANGE
            var cNumber = 10000;
            var name = "Bevila";
            var age = 33;
            var dob = new DateTime(1987, 8, 22);
            var zipCode = 9999;

            // ACT
            var customer = new Customer(cNumber, name, age, dob, zipCode);

            // ASSERT
            Assert.Equal(cNumber, customer.CNumber);
            Assert.Equal(name, customer.Name);
            Assert.Equal(age, customer.Age);
            Assert.Equal(dob, customer.Dob);
            Assert.Equal(zipCode, customer.ZipCode);
        }

        [Fact(DisplayName = "Should not create a customer if cNumber is zero")]
        public void Customer_ShouldNotCreateCustomerIfCNumberIsZero()
        {
            // ARRANGE
            var cNumber = 0;

            // ACT
            // ASSERT
            Assert.Throws<DomainException>(() => new Customer(cNumber, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>()));
        }

        [Fact(DisplayName = "Should add a zone to a customer")]
        public void Customer_ShouldAddAZone()
        {
            // ARRANGE
            var cNumber = 10000;
            var name = "Bevila";
            var age = 33;
            var dob = new DateTime(1987, 8, 22);
            var zipCode = 9999;

            // ACT
            var customer = new Customer(cNumber, name, age, dob, zipCode);
            customer.AddZone(Zone.Yellow);

            // ASSERT
            Assert.Equal(Zone.Yellow, customer.Zone);
        }
    }
}
