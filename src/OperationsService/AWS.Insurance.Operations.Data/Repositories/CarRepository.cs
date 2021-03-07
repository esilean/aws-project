using AWS.Insurance.Operations.Data.Repositories.Interfaces;
using AWS.Insurance.Operations.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Data.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly DbContext _context;

        public CarRepository(DbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Car car)
        {
            await _context.AddAsync(car);
        }
    }
}