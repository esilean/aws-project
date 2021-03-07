using AWS.Insurance.STS.Application.App;
using AWS.Insurance.STS.Application.App.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AWS.Insurance.STS.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> Login([FromBody] Auth.Query query)
        {
            return await _mediator.Send(query);
        }
    }
}
