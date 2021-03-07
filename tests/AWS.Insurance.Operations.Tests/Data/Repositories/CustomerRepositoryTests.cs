using AWS.Insurance.Operations.Data.Context;
using AWS.Insurance.Operations.Data.Repositories;
using AWS.Insurance.Operations.Domain.Models;
using AWS.Insurance.Operations.Tests.Data.AConfig;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AWS.Insurance.Operations.Tests.Data.Repositories
{
    public class CustomerRepositoryTests : DbConfigForTest
    {

        private readonly CustomerRepository _customerRepository;
        private readonly OpDbContext _context;

        public CustomerRepositoryTests()
        {
            _context = new OpDbContext(ContextOptions);
            _customerRepository = new CustomerRepository(_context);
        }


        [Fact(DisplayName = "Should add a customer to DB")]
        public async Task Customer_ShouldAddToDB()
        {
            // ARRANGE
            var customer = new Customer(1234, "name", 33, DateTime.Today, 9999);

            // ACT
            await _customerRepository.AddAsync(customer);
            await _context.SaveChangesAsync();

            // ASSERT
            Assert.True(_context.Set<Customer>().Any(x => x.Id == customer.Id));
        }

        [Fact(DisplayName = "Should throw an error uf customer is null")]
        public async Task Customer_ShouldThrowAnErrorCustomerNull()
        {
            // ARRANGE
            Customer customer = null;

            // ACT
            // ASSERT
            await Assert.ThrowsAsync<ArgumentNullException>(() => _customerRepository.AddAsync(customer));
        }

        [Fact(DisplayName = "Should get a customer by Id from DB")]
        public async Task Customer_ShouldGetACustomerByIdFromDB()
        {
            // ARRANGE
            var customer = new Customer(1234, "name", 33, DateTime.Today, 9999);
            await _customerRepository.AddAsync(customer);
            await _context.SaveChangesAsync();

            // ACT
            var response = await _customerRepository.GetByIdAsync(customer.Id);

            // ASSERT
            Assert.True(_context.Set<Customer>().Any(x => x.Id == response.Id));
        }

        [Fact(DisplayName = "Should get a customer by CNumber from DB")]
        public async Task Customer_ShouldGetACustomerByCNumberFromDB()
        {
            // ARRANGE
            var cNumber = 6666;
            var customer = new Customer(cNumber, "name", 33, DateTime.Today, 9999);
            await _customerRepository.AddAsync(customer);
            await _context.SaveChangesAsync();

            // ACT
            var response = await _customerRepository.GetByCNumberAsync(customer.CNumber);

            // ASSERT
            Assert.True(_context.Set<Customer>().Any(x => x.CNumber == response.CNumber));
        }
    }
}
