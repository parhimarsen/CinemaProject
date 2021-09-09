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
    public class TicketsController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly AmenityService _amenityService;

        public TicketsController(TicketService ticketService, AmenityService amenityService)
        {
            _ticketService = ticketService;
            _amenityService = amenityService;
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(Guid id)
        {
            Ticket ticket = await _ticketService.GetAsync(id);

            return Ok(ticket);
        }

        // GET: api/Tickets/5/Amenities
        [HttpGet("{id}/Amenities")]
        public async Task<IActionResult> GetAmenitiesOfTicket(Guid id)
        {
            Amenity[] amenitiesOfTicket = await _amenityService.GetAllOfTicketAsync(id);

            return Ok(amenitiesOfTicket);
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<IActionResult> PostTicket(Ticket model)
        {
            Ticket response = await _ticketService.InsertAsync(model);

            return Ok(response);
        }

        // POST: api/Tickets/5/Amenities/5
        [HttpPost("{ticketId}/Amenities/{amenityId}")]
        public async Task<IActionResult> PostAmenity(Guid ticketId, Guid amenityId)
        {
            bool response = await _amenityService.InsertToTicketAsync(ticketId, amenityId);

            if (response)
                return Ok("Amenity added to ticket");
            else
                return BadRequest("This Ticket of Amenity not exist");
        }

        // POST: api/Tickets/5/confirm
        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> ConfirmPayment(Guid id)
        {
            ConfirmPaymentResponse response = await _ticketService.ConfirmPayment(id);

            if (!response.IsConfirm)
                return BadRequest(response.Response);
            else
                return Ok(response.Response);
        }

        // PUT: api/Tickets
        [HttpPut]
        public async Task<IActionResult> PutTicket(Ticket model)
        {
            await _ticketService.UpdateAsync(model);

            return NoContent();
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            await _ticketService.RemoveAsync(id);

            return NoContent();
        }

        // DELETE: api/Tickets/5/Amenities/5
        [HttpDelete("{ticketId}/Amenities/{amenityId}")]
        public async Task<IActionResult> DeleteAmenity(Guid ticketId, Guid amenityId)
        {
            await _amenityService.RemoveFromTicketAsync(ticketId, amenityId);

            return NoContent();
        }
    }
}
