using AWS.Insurance.Locations.Domain.Interfaces;
using AWS.Insurance.Locations.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AWS.Insurance.Locations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILogger<LocationsController> _logger;
        private readonly ILocationService _locationService;

        public LocationsController(ILogger<LocationsController> logger,
                                   ILocationService locationService)
        {
            _logger = logger;
            _locationService = locationService;
        }

        /// <summary>
        /// Finds the a zone by ZipCode.
        /// </summary>     
        /// <returns>A zone if exists</returns>
        /// <response code="200">Returns the zone</response>
        /// <response code="401">Returns unauthorized</response>
        /// <response code="404">Returns not found</response>
        [HttpGet("{zipCode}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Authorize]
        public async Task<Location> GetZone(int zipCode)
        {
            _logger
                .LogInformation("Looking for zone at ZipCode {zipCode}",
                                zipCode);

            return await _locationService.GetZone(zipCode);
        }
    }
}