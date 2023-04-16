using MediatR;
using Microsoft.AspNetCore.Mvc;
using Report.Application.Features.Queries;

namespace Report.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public string Get()
        {
            return "Report Service is Working!";
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetByCorrelationId([FromQuery] GetEvent.Query query)
        {
            return Ok(await _mediator.Send(query));
        }
    }
}
