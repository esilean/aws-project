using AWS.Insurance.Operations.Domain.Models;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Data.Repositories.Interfaces
{
    public interface ICarRepository
    {
         Task AddAsync(Car car);
    }
}