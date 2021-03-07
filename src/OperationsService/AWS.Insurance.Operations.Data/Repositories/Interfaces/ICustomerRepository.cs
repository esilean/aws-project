using AWS.Insurance.Operations.Domain.Models;
using System;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Data.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(Guid id);
        Task<Customer> GetByCNumberAsync(int cNumber);
        Task AddAsync(Customer customer);
    }
}