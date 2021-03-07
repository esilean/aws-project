using AWS.Insurance.Operations.Application.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace AWS.Insurance.Operations.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ILogger<CustomersController> _logger;
        private readonly IMediator _mediator;

        public CustomersController(ILogger<CustomersController> logger,
                              IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Create a customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/customers
        ///     {
        ///         "CNumber": "08d89f9c-9409-4ca8-8236-3a436eba207d",
        ///         "Name": "Rena",
        ///         "age": 30,
        ///         "dob": "1990-07-02"
        ///         "zipCode": 9999
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
                .LogInformation("Creating a customer. Data: CNumber: {cNumber}, Name: {name}, Age: {age}, Dob: {dob}, ZipCode: {zipCode}",
                                command.CNumber,
                                command.Name,
                                command.Age,
                                command.Dob,
                                command.ZipCode);

            return await _mediator.Send(command);
        }
    }
}
