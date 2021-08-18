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
    public class CinemasController : ControllerBase
    {
        private readonly CinemaService _cinemaService;
        private readonly HallService _hallService;
        private readonly CityService _cityService;
        private readonly TypeOfSeatService _typeOfSeatService;

        public CinemasController(
            CinemaService cinemaService,
            CityService cityService,
            HallService hallService,
            TypeOfSeatService typeOfSeatService)
        {
            _cinemaService = cinemaService;
            _cityService = cityService;
            _hallService = hallService;
            _typeOfSeatService = typeOfSeatService;
        }

        // GET: api/Cinemas
        [HttpGet]
        [EnableQuery]
        public IQueryable<Cinema> GetCinemas()
        {
            return _cinemaService.GetAll();
        }

        // GET: api/Cinemas/5/Halls
        [HttpGet("{id}/Halls")]
        [EnableQuery]
        public async Task<IQueryable<Hall>> GetHalls(Guid id)
        {
            return await _hallService.GetAllOfCinemaAsync(id);
        }

        // GET: api/Cinemas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCinema(Guid id)
        {
            Cinema response = await _cinemaService.GetAsync(id);

            return Ok(response);
        }

        // GET: api/Cinemas/5/City
        [HttpGet("{id}/City")]
        public async Task<IActionResult> GetCity(Guid id)
        {
            City response = await _cityService.GetOfCinemaAsync(id);

            return Ok(response);
        }

        // POST: api/Cinemas
        [HttpPost]
        public async Task<IActionResult> PostCinema(Cinema model)
        {
            Cinema response = await _cinemaService.InsertAsync(model);

            return Ok(response);
        }

        // PUT: api/Cinemas
        [HttpPut]
        public async Task<IActionResult> PutCinema(Cinema model)
        {
            await _cinemaService.UpdateAsync(model);

            return NoContent();
        }

        // DELETE: api/Cinemas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCinema(Guid id)
        {
            await _cinemaService.RemoveAsync(id);

            return NoContent();
        }

        // POST: api/Cinemas/5/TypesOfSeatRange
        [HttpPost("{id}/TypesOfSeatRange")]
        public async Task<IActionResult> DeleteCinema(Guid id, TypeOfSeat[] model)
        {
            await _typeOfSeatService.RemoveRangeAsync(id, model);

            return NoContent();
        }
    }
}
