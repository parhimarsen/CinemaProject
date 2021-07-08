using Microsoft.AspNetCore.Mvc;

namespace CinemaProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/Users
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok();
        }
    }
}
