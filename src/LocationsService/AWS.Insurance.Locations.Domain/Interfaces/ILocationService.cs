using AWS.Insurance.Locations.Domain.Models;
using System.Threading.Tasks;

namespace AWS.Insurance.Locations.Domain.Interfaces
{
    public interface ILocationService
    {
        Task<Location> GetZone(int zipCode);
    }
}