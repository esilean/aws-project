using AWS.Insurance.Operations.Application.Cars;
using AWS.Insurance.Operations.Application.Map;
using AWS.Insurance.Operations.Domain.Models;
using AWS.Insurance.Operations.Domain.Models.Enums;
using System;
using Xunit;

namespace AWS.Insurance.Operations.Tests.Application.Map
{
    public class MappingTests
    {
        [Fact(DisplayName = "Should map a command to a domain")]
        public void Map_CommandToDomain()
        {
            // ARRANGE
            var command = new Create.Command
            {
                CustomerId = Guid.NewGuid(),
                BranchType = (int)BranchType.BMW,
                Name = "ZZ",
                Year = 2000
            };

            // ACT
            var sut = Mapping.Map<Create.Command, Car>(command);

            // ASSERT
            Assert.Equal(command.CustomerId, sut.CustomerId);
            Assert.Equal(command.BranchType, (int)sut.BranchType);
            Assert.Equal(command.Name, sut.Name);
            Assert.Equal(command.Year, sut.Year);
        }
    }
}
