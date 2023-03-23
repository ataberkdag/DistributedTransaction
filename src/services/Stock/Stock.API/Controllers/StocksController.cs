using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features.Commands;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StocksController(IMediator mediator)
        {
            _mediator= mediator;
        }

        [HttpGet()]
        [AllowAnonymous]
        public string Get()
        {
            return "Stocks Service is Working!";
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DecreaseStock(DecreaseStock.Command command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public string AdminGet()
        {
            return "Admin | Service is Working!";
        }
    }
}
