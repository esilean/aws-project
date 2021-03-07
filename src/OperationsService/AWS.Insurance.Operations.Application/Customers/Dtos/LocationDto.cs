using AWS.Insurance.Operations.Domain.Models.Enums;

namespace AWS.Insurance.Operations.Application.Customers.Dtos
{
    public class LocationDto
    {
        public int ZipCode { get; set; }    
        public Zone Zone { get; set; }
    }
}