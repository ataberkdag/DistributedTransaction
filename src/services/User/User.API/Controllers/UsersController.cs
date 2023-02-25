using Microsoft.AspNetCore.Mvc;
using User.API.Contracts;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        [HttpGet]
        public IActionResult CheckUser([FromQuery] CheckUserRequest request)
        {
            return Ok(new CheckUserResponse() { 
                ActivationDate = DateTime.Now,
                IsActive = true
            });
        }
    }
}
