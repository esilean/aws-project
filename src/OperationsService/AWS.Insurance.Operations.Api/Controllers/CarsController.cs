using AWS.Insurance.Operations.Application.Cars;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ILogger<CarsController> _logger;
        private readonly IMediator _mediator;

        public CarsController(ILogger<CarsController> logger,
                              IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Create a car related to a customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/cars
        ///     {
        ///         "customerId": "08d89f9c-9409-4ca8-8236-3a436eba207d",
        ///         "branchType": 1,
        ///         "name": "ZZ",
        ///         "year": 2001
        ///     }
        ///
        /// </remarks>        
        /// <param name="command"></param>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<Unit>> Create([FromBody] Create.Command command)
        {
            _logger
                .LogInformation("Creating a car. Data: CustomerId: {customerId}, Branch: {branchType}, Name: {name}, Year: {year}",
                                command.CustomerId,
                                command.BranchType,
                                command.Name,
                                command.Year);

            return await _mediator.Send(command);
        }
    }
}
