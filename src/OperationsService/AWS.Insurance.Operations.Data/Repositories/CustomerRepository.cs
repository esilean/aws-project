using AWS.Insurance.Operations.Data.Context;
using AWS.Insurance.Operations.Data.Repositories.Interfaces;
using AWS.Insurance.Operations.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly OpDbContext _context;

        public CustomerRepository(OpDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Customer> GetByCNumberAsync(int cNumber)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.CNumber == cNumber);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
        }
    }
}