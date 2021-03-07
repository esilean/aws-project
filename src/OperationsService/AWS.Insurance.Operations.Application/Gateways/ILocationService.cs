using AWS.Insurance.Operations.Application.Customers.Dtos;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Application.Gateways
{
    public interface ILocationService
    {
         Task<LocationDto> GetLocation(int zipCode);
    }
}