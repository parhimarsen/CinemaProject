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
    public class FilmsController : ControllerBase
    {
        private readonly FilmService _filmService;
        private readonly ActorService _actorService;
        private readonly GenreService _genreService;

        public FilmsController(
            FilmService filmService,
            ActorService actorService,
            GenreService genreService)
        {
            _filmService = filmService;
            _actorService = actorService;
            _genreService = genreService;
        }

        // GET: api/Films
        [HttpGet]
        [EnableQuery]
        public IQueryable<Film> GetFilms()
        {
            return _filmService.GetAll();
        }

        // GET: api/Films/5/Actors
        [HttpGet("{id}/Actors")]
        public async Task<IActionResult> GetActors(Guid id)
        {
            Actor[] response = await _actorService.GetAllOfFilm(id);

            return Ok(response);
        }

        // GET: api/Films/5/Genres
        [HttpGet("{id}/Genres")]
        [EnableQuery]
        public async Task<IActionResult> GetGenres(Guid id)
        {
            Genre[] response = await _genreService.GetAllOfFilm(id);

            return Ok(response);
        }

        // POST: api/Films
        [HttpPost]
        public async Task<IActionResult> PostFilm(Film model)
        {
            Film response = await _filmService.InsertAsync(model);

            return Ok(response);
        }

        // POST: api/Films/5/Actors
        [HttpPost("{id}")]
        public async Task<IActionResult> PostActors(Guid id, Actor[] model)
        {
            Actor[] response = await _actorService.InsertToFilmAsync(id, model);

            return Ok(response);
        }

        // PUT: api/Films
        [HttpPut]
        public async Task<IActionResult> PutFilm(Film model)
        {
            await _filmService.UpdateAsync(model);

            return NoContent();
        }

        // DELETE: api/Films/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFilm(Guid id)
        {
            await _filmService.RemoveAsync(id);

            return NoContent();
        }
    }
}
