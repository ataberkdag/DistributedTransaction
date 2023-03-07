using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Payment Service is working!";
        }
    }
}
