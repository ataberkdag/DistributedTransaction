using Microsoft.AspNetCore.Mvc;

namespace Report.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Report Service is Working!";
        }
    }
}
