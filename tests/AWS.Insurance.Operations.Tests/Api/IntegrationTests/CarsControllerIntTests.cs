using AWS.Insurance.Operations.Api;
using AWS.Insurance.Operations.Application.Cars;
using AWS.Insurance.Operations.Data.Context;
using AWS.Insurance.Operations.Domain.Models;
using AWS.Insurance.Operations.Domain.Models.Enums;
using AWS.Insurance.Operations.Tests.Api.AConfig;
using AWS.Insurance.Operations.Tests.Api.AConfig.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Insurance.Locations.Tests.Api.IntegrationTests
{
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class CarsControllerIntTests
    {
        private readonly IntegrationTestsFixture<Startup> _testsFixture;
        private readonly OpDbContext _context;

        public CarsControllerIntTests(IntegrationTestsFixture<Startup> testsFixture)
        {
            _testsFixture = testsFixture;

            var builder = new DbContextOptionsBuilder<OpDbContext>();
            builder.UseInMemoryDatabase("OpInMemory");
            _context = new OpDbContext(builder.Options);
        }

        [Fact(DisplayName = "Should return a success 200 Status")]
        public async Task Cars_WhenCommandValid_ShouldReturnA200StatusCode()
        {
            // ARRANGE
            await _context.Customers.AddAsync(new Customer(4444, "Bevila", 33, DateTime.Today, 9999));
            await _context.SaveChangesAsync();
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.CNumber == 4444);


            var command = new Create.Command
            {
                CustomerId = customer.Id,
                BranchType = (int)BranchType.BMW,
                Name = "ZZ",
                Year = 2000
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // ACT
            var response = await _testsFixture.Client.PostAsync($"/api/cars", stringContent);

            // ASSERT
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Should return an error 404 Status if Customer does not exist")]
        public async Task Customer_WhenCnumberExists_ShouldReturnA400StatusCode()
        {
            // ARRANGE
            var command = new Create.Command
            {
                CustomerId = Guid.NewGuid(),
                BranchType = (int)BranchType.BMW,
                Name = "ZZ",
                Year = 2000
            };

            var stringContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");

            // ACT
            var response = await _testsFixture.Client.PostAsync($"/api/cars", stringContent);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ErrorDto>(result);

            // ASSERT
            Assert.Equal(404, (int)response.StatusCode);
            Assert.Equal("Customer has not been found.", data.Errors.Message);
        }

    }
}
