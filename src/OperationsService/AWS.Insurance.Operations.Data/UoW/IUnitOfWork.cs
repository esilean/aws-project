using AWS.Insurance.Operations.Data.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Data.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        ICarRepository Cars { get; }
        Task<bool> SaveAsync();
    }
}