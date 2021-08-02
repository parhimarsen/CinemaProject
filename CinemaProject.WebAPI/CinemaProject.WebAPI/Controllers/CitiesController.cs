using CinemaProject.BLL.Models;
using CinemaProject.BLL.Services;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly CityService _cityService;
        private readonly CinemaService _cinemaService;

        public CitiesController(CityService cityService, CinemaService cinemaService)
        {
            _cityService = cityService;
            _cinemaService = cinemaService;
        }

        // GET: api/Cities
        [HttpGet]
        public IActionResult GetCities()
        {
            List<City> response = _cityService.GetAll();

            return Ok(response);
        }

        // GET: api/Cities/5/Cinemas
        [HttpGet("{id}/Cinemas")]
        [EnableQuery]
        public async Task<IQueryable<Cinema>> GetCinemas(Guid id)
        {
            return await _cinemaService.GetOfCity(id);
        }

        // POST: api/Cities
        [HttpPost]
        public async Task<IActionResult> PostCity(City model)
        {
            City response = await _cityService.InsertAsync(model);

            return Ok(response);
        }

        // PUT: api/Cities
        [HttpPut]
        public async Task<IActionResult> PutCity(City model)
        {
            await _cityService.UpdateAsync(model);

            return NoContent();
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCity(Guid id)
        {
            await _cityService.RemoveAsync(id);

            return NoContent();
        }
    }
}
