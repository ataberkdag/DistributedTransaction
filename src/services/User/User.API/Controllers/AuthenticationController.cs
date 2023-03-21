using MediatR;
using Microsoft.AspNetCore.Mvc;
using User.BL.Features.Commands;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateToken")]
        public async Task<IActionResult> CreateToken(CreateToken.Command command)
        {
            return Ok(await _mediator.Send(command));
        }

    }
}
