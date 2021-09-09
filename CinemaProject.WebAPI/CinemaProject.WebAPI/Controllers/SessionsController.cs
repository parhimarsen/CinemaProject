using CinemaProject.BLL.Models;
using CinemaProject.BLL.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SessionsController : ControllerBase
    {
        private readonly SessionService _sessionService;
        private readonly AmenityService _amenityService;

        public SessionsController(SessionService sessionService, AmenityService amenityService)
        {
            _sessionService = sessionService;
            _amenityService = amenityService;
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

        // GET: api/Sessions/5/Amenities
        [HttpGet("{id}/Amenities")]
        public async Task<IActionResult> GetAmenitiesOfSession(Guid id)
        {
            Amenity[] amenitiesOfSession = await _amenityService.GetAllOfSessionAsync(id);

            return Ok(amenitiesOfSession);
        }

        // POST: api/Sessions
        [HttpPost]
        public async Task<IActionResult> PostSession(Session model)
        {
            Session response = await _sessionService.InsertAsync(model);

            return Ok(response);
        }

        // POST: api/Sessions/5/Amenities
        [HttpPost("{sessionId}/Amenities")]
        public async Task<IActionResult> PostAmenity(Guid sessionId, Amenity model)
        {
            bool response = await _amenityService.InsertToSessionAsync(sessionId, model);

            if (response)
                return Ok("Amenity added to session");
            else
                return BadRequest("This Session of Amenity not exist");
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

        // DELETE: api/Sessions/5/Amenities/5
        [HttpDelete("{sessionId}/Amenities/{amenityId}")]
        public async Task<IActionResult> DeleteAmenity(Guid sessionId, Guid amenityId)
        {
            await _amenityService.RemoveFromSessionAsync(sessionId, amenityId);

            return NoContent();
        }
    }
}
