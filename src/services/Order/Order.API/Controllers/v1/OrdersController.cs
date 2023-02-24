using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Features.Commands;

namespace Order.API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> PlaceOrder(PlaceOrder.Command command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}
