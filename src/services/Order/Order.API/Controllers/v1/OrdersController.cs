using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Features.Commands;

namespace Order.API.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Places an Order
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created Order</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /PlaceOrder
        ///     {
        ///        "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///        "orderItems": [
        ///          {
        ///            "itemId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///            "quantity": 10
        ///          }
        ///        ]
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Route("PlaceOrder")]
        [ProducesResponseType(typeof(PlaceOrder.Response), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PlaceOrder(PlaceOrder.Command command)
        {
            return CreatedAtAction(nameof(PlaceOrder), await _mediator.Send(command));
        }
    }
}
