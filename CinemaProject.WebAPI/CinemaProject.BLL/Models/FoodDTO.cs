using System;

namespace CinemaProject.BLL.Models
{
    public class FoodDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public TicketDTO Ticket { get; set; }
    }
}
