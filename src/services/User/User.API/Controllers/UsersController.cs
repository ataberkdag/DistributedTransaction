using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.BL.Features.Commands;

namespace User.API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateUser")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(CreateUser.Command command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("AddRoleToUser")]
        public async Task<IActionResult> AddRoleToUser(AddRoleToUser.Command command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
