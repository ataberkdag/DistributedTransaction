using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Ataberk")]
    public class TestsController : ControllerBase
    {
        [HttpGet("Access")]
        public string Get()
        {
            return "User/Ataberk accessed!";
        }

        [HttpGet("AdminAccess")]
        [Authorize(Roles = "Admin")]
        public string AdminAccess()
        {
            return "Admin accessed!";
        }
    }
}
