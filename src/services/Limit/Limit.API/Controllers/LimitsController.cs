using Limit.API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Limit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LimitsController : ControllerBase
    {
        [HttpGet]
        public IActionResult CheckLimits([FromQuery] CheckLimitRequest request)
        {
            return Ok(new CheckLimitResponse()
            {
                IsLimitExceeded = false
            });
        }
    }
}
