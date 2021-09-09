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
    public class AmenitiesController : ControllerBase
    {
        private readonly AmenityService _amenityService;

        public AmenitiesController(AmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        // GET: api/Amenities
        [HttpGet]
        [EnableQuery]
        public IQueryable<Amenity> GetAmenities()
        {
            return _amenityService.GetAll();
        }

        // POST: api/Amenities
        [HttpPost]
        public async Task<IActionResult> PostAmenity(Amenity model)
        {
            Amenity response = await _amenityService.InsertAsync(model);

            return Ok(response);
        }

        // PUT: api/Amenities
        [HttpPut]
        public async Task<IActionResult> PutAmenity(Amenity model)
        {
            await _amenityService.UpdateAsync(model);

            return NoContent();
        }

        // DELETE: api/Amenities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmenity(Guid id)
        {
            await _amenityService.RemoveAsync(id);

            return NoContent();
        }
    }
}
