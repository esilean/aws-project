using AWS.Insurance.Locations.Domain.Errors;
using AWS.Insurance.Locations.Domain.Interfaces;
using AWS.Insurance.Locations.Domain.Models;
using AWS.Insurance.Locations.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AWS.Insurance.Locations.Infra
{
    public class LocationService : ILocationService
    {
        public async Task<Location> GetZone(int zipCode)
        {
            var locations = await LoadLocations();
            var location = locations.FirstOrDefault(x => x.ZipCode == zipCode);

            if (location == null)
                throw new RestException(HttpStatusCode.NotFound, new { message = "There is no Zone for this ZipCode." });

            return location;
        }

        private async Task<IEnumerable<Location>> LoadLocations()
        {
            var locations = new List<Location>();
            for (int i = 1; i <= 10000; i++)
                locations.Add(new Location(i, (Zone)GetRandomZone(1, 3)));

            //simulate external delay
            await Task.Delay(500);

            return await Task.FromResult(locations);
        }

        private int GetRandomZone(int minimum, int maximum)
        {
            Random random = new Random();
            return random.Next(minimum, maximum + 1);
        }
    }
}