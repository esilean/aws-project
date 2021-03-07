using AWS.Insurance.Operations.Application.Customers.Dtos;
using AWS.Insurance.Operations.Application.Gateways;
using AWS.Insurance.Operations.Domain.Models.Enums;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Tests.Api.AConfig.Stubs
{
    public class LocationServiceStub : ILocationService
    {
        public Task<LocationDto> GetLocation(int zipCode)
        {
            return Task.FromResult(new LocationDto { ZipCode = zipCode, Zone = Zone.Green });
        }
    }
}
