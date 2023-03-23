using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public string Get()
        {
            return "Payment Service is working!";
        }

        [HttpGet("AdminGet")]
        [Authorize(Roles = "Admin")]
        public string AdminGet()
        {
            return "Admin | Service is Working!";
        }
    }
}
