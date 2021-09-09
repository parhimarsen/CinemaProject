using CinemaProject.BLL.Models;
using CinemaProject.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CinemaProject.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : ControllerBase
    {
        private readonly SeatService _seatService;

        public SeatsController(SeatService seatService)
        {
            _seatService = seatService;
        }

        // GET: api/Seats/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeat(Guid id)
        {
            Seat response = await _seatService.GetAsync(id);

            return Ok(response);
        }

        // POST: api/Seats
        [HttpPost]
        public async Task<IActionResult> PostSeat(Seat model)
        {
            Seat response = await _seatService.InsertAsync(model);

            return Ok(response);
        }

        // PUT: api/Seats
        [HttpPut]
        public async Task<IActionResult> PutSeat(Seat model)
        {
            await _seatService.UpdateAsync(model);

            return NoContent();
        }

        // DELETE: api/Seats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat(Guid id)
        {
            await _seatService.RemoveAsync(id);

            return NoContent();
        }
    }
}
