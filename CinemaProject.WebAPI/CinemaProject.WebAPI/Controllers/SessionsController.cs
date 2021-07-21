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
    public class SessionsController : ControllerBase
    {
        private readonly SessionService _sessionService;

        public SessionsController(SessionService sessionService)
        {
            _sessionService = sessionService;
        }

        // GET: api/Sessions
        [HttpGet]
        [EnableQuery]
        public IQueryable<Session> GetSessions()
        {
            return _sessionService.GetAll();
        }

        // GET: api/Sessions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSession(Guid id)
        {
            Session response = await _sessionService.GetAsync(id);

            return Ok(response);
        }

        // POST: api/Sessions
        [HttpPost]
        public async Task<IActionResult> PostSession(Session model)
        {
            Session response = await _sessionService.InsertAsync(model);

            return Ok(response);
        }

        // PUT: api/Sessions
        [HttpPut]
        public async Task<IActionResult> PutSession(Session model)
        {
            await _sessionService.UpdateAsync(model);

            return NoContent();
        }

        // DELETE: api/Sessions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(Guid id)
        {
            await _sessionService.RemoveAsync(id);

            return NoContent();
        }
    }
}
