using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.BL.Features.Commands;

namespace User.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRole.Command command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
