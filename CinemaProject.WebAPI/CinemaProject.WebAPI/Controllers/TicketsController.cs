using CinemaProject.BLL.Models;
using CinemaProject.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CinemaProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly TicketService _ticketService;
        private readonly FoodService _foodService;

        public TicketsController(TicketService ticketService, FoodService foodService)
        {
            _ticketService = ticketService;
            _foodService = foodService;
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(Guid id)
        {
            Ticket ticket = await _ticketService.GetAsync(id);

            return Ok(ticket);
        }

        // GET: api/Tickets/5/Foods
        [HttpGet("{id}/Foods")]
        public async Task<IActionResult> GetFoodOfTicket(Guid id)
        {
            Food[] foodsOfTicket = await _foodService.GetAllOfTicketAsync(id);

            return Ok(foodsOfTicket);
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<IActionResult> PostTicket(Ticket model)
        {
            Ticket response = await _ticketService.InsertAsync(model);

            return Ok(response);
        }

        // POST: api/Tickets/5/Foods/5
        [HttpPost("{ticketId}/Foods/{foodId}")]
        public async Task<IActionResult> PostFood(Guid ticketId, Guid foodId)
        {
            bool response = await _foodService.InsertToTicketAsync(ticketId, foodId);

            if (response)
                return Ok("Food added to ticket");
            else
                return BadRequest("This Ticket of Food not exist");
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

        // DELETE: api/Tickets/5/Foods/5
        [HttpDelete("{ticketId}/Foods/{foodId}")]
        public async Task<IActionResult> DeleteFood(Guid ticketId, Guid foodId)
        {
            await _foodService.RemoveFromTicketAsync(ticketId, foodId);

            return NoContent();
        }
    }
}
