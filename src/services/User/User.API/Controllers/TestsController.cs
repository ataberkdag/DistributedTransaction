using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        [HttpGet("Access")]
        [AllowAnonymous]
        public string Get()
        {
            return "User accessed!";
        }

        [HttpGet("AdminAccess")]
        [Authorize(Roles = "Admin")]
        public string AdminAccess()
        {
            return "Admin accessed!";
        }
    }
}
