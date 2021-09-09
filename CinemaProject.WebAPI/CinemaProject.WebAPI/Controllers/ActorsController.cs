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
    public class ActorsController : ControllerBase
    {
        private readonly ActorService _actorService;

        public ActorsController(ActorService actorService)
        {
            _actorService = actorService;
        }

        // GET: api/Actors
        [HttpGet]
        [EnableQuery]
        public IQueryable<Actor> GetActors()
        {
            return _actorService.GetAll();
        }

        // POST: api/Actors
        [HttpPost]
        public async Task<IActionResult> PostActor(Actor model)
        {
            Actor response = await _actorService.InsertAsync(model);

            return Ok(response);
        }

        // PUT: api/Actors
        [HttpPut]
        public async Task<IActionResult> PutActor(Actor model)
        {
            await _actorService.UpdateAsync(model);

            return NoContent();
        }

        // DELETE: api/Actors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(Guid id)
        {
            await _actorService.RemoveAsync(id);

            return NoContent();
        }
    }
}
