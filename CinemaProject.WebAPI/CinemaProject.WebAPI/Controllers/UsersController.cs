using CinemaProject.BLL.Models;
using CinemaProject.BLL.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly TicketService _ticketService;

        public UsersController(UserService userService, TicketService ticketService)
        {
            _userService = userService;
            _ticketService = ticketService;
        }

        // POST: api/Users/authenticate
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            AuthenticateResponse response = await _userService.Authenticate(model, Request.HttpContext.Connection.RemoteIpAddress.ToString());

            if (response == null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(response);
        }

        // POST: api/Users/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest model)
        {
            AuthenticateResponse response = await _userService.Register(model, Request.HttpContext.Connection.RemoteIpAddress.ToString());

            return Ok(response);
        }

        // POST: api/Users/refresh-token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest model)
        {
            string refreshToken = model.JwtToken;
            AuthenticateResponse response = await _userService.RefreshToken(refreshToken, Request.HttpContext.Connection.RemoteIpAddress.ToString());

            if (response == null)
            {
                return Unauthorized(new { message = "Invalid token" });
            }

            return Ok(response);
        }

        // POST: api/Users/revoke-token
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] TokenRequest model)
        {
            string token = model.JwtToken;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            bool response = await _userService.RevokeToken(token, Request.HttpContext.Connection.RemoteIpAddress.ToString());

            if (!response)
            {
                return NotFound(new { message = "Token not found" });
            }

            return Ok(new { message = "Token revoked" });
        }

        // GET: api/Users
        [HttpGet]
        [EnableQuery]
        public IQueryable<User> GetUsers()
        {
            return _userService.GetAll();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public IActionResult GetUser(Guid id)
        {
            User user = _userService.Get(id);

            return Ok(user);
        }

        // GET: api/Users/5/Tickets
        [HttpGet("{id}/Tickets")]
        [EnableQuery]
        public async Task<IActionResult> GetTickets(Guid id)
        {
            IQueryable<Ticket> ticketsOfUserQuery = await _ticketService.GetAllOfUserAsync(id);

            return Ok(ticketsOfUserQuery);
        }

        // PUT: api/Users
        [HttpPut]
        public async Task<IActionResult> PutUser(User user)
        {
            await _userService.UpdateAsync(user);

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.RemoveAsync(id);

            return NoContent();
        }
    }
}
