using AWS.Insurance.Operations.Data.Context;
using AWS.Insurance.Operations.Data.Repositories;
using AWS.Insurance.Operations.Domain.Models;
using AWS.Insurance.Operations.Domain.Models.Enums;
using AWS.Insurance.Operations.Tests.Data.AConfig;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Insurance.Operations.Tests.Data.Repositories
{
    public class CarRepositoryTests : DbConfigForTest
    {

        private readonly CarRepository _carRepository;
        private readonly OpDbContext _context;

        public CarRepositoryTests()
        {
            _context = new OpDbContext(ContextOptions);
            _carRepository = new CarRepository(_context);
        }


        [Fact(DisplayName = "Should add a car to DB")]
        public async Task Car_ShouldAddToDB()
        {
            // ARRANGE
            var customer = new Customer(1234, "name", 33, DateTime.Today, 9999);
            await _context.AddAsync(customer);
            var car = new Car(customer.Id, BranchType.BMW, "ZZ", 2020);

            // ACT
            await _carRepository.AddAsync(car);
            await _context.SaveChangesAsync();

            // ASSERT
            Assert.True(_context.Set<Car>().Any(x => x.Id == car.Id));
        }

        [Fact(DisplayName = "Should throw an error if car is null")]
        public async Task Car_ShouldThrowAnErrorCarNull()
        {
            // ARRANGE
            Car car = null;

            // ACT
            // ASSERT
            await Assert.ThrowsAsync<ArgumentNullException>(() => _carRepository.AddAsync(car));
        }

    }
}
