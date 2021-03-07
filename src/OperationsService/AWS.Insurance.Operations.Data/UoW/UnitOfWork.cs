using AWS.Insurance.Operations.Data.Context;
using AWS.Insurance.Operations.Data.Repositories;
using AWS.Insurance.Operations.Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Data.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OpDbContext _context;

        public UnitOfWork(OpDbContext context)
        {
            _context = context;
            Customers = new CustomerRepository(_context);
            Cars = new CarRepository(_context);
        }

        public ICustomerRepository Customers { get; }
        public ICarRepository Cars { get; }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}