using AWS.Insurance.Locations.Domain.Errors;
using AWS.Insurance.Locations.Domain.Models.Enums;

namespace AWS.Insurance.Locations.Domain.Models
{
    public class Location
    {
        public int ZipCode { get; private set; }
        public Zone Zone { get; private set; }

        public Location(int zipCode, Zone zone)
        {
            if (zipCode <= 0 || zipCode > 10000)
                throw new DomainException(ErrorConstants.ZipCodeLocationDomainError);

            ZipCode = zipCode;
            Zone = zone;
        }
    }
}