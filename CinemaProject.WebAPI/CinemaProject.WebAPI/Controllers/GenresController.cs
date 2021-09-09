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
    public class GenresController : ControllerBase
    {
        private readonly GenreService _genreService;

        public GenresController(GenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: api/Genres
        [HttpGet]
        [EnableQuery]
        public IQueryable<Genre> GetGenres()
        {
            return _genreService.GetAll();
        }

        // POST: api/Genres
        [HttpPost]
        public async Task<IActionResult> PostGenre(Genre model)
        {
            Genre response = await _genreService.InsertAsync(model);

            return Ok(response);
        }

        // PUT: api/Genres
        [HttpPut]
        public async Task<IActionResult> PutGenre(Genre model)
        {
            await _genreService.UpdateAsync(model);

            return NoContent();
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(Guid id)
        {
            await _genreService.RemoveAsync(id);

            return NoContent();
        }
    }
}
