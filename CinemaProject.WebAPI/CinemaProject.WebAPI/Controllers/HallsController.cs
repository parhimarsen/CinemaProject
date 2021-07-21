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
    public class HallsController : ControllerBase
    {
        private readonly HallService _hallService;
        private readonly SeatService _seatService;

        public HallsController(HallService hallService, SeatService seatService)
        {
            _hallService = hallService;
            _seatService = seatService;
        }

        // GET: api/Halls/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHall(Guid id)
        {
            Hall response = await _hallService.GetAsync(id);

            return Ok(response);
        }

        // GET: api/Halls/5/Seats
        [HttpGet("{id}/Seats")]
        [EnableQuery]
        public async Task<IQueryable<Seat>> GetSeats(Guid id)
        {
            return await _seatService.GetAllOfHallAsync(id);
        }

        // POST: api/Halls
        [HttpPost]
        public async Task<IActionResult> PostHall(Hall model)
        {
            Hall response = await _hallService.InsertAsync(model);

            return Ok(response);
        }

        // POST: api/Halls/5/Seats
        [HttpPost("{id}/Seats")]
        public async Task<IActionResult> PostSeats(Guid id, Seat[] model)
        {
            Seat[] response = await _seatService.InsertRangeAsync(id, model);

            return Ok(response);
        }

        // PUT: api/Halls
        [HttpPut]
        public async Task<IActionResult> PutHall(Hall model)
        {
            await _hallService.UpdateAsync(model);

            return NoContent();
        }

        // DELETE: api/Halls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHall(Guid id)
        {
            await _hallService.RemoveAsync(id);

            return NoContent();
        }
    }
}
