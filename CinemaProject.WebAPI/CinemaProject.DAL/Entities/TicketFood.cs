using System;

namespace CinemaProject.DAL.Entities
{
    public class TicketFood
    {
        public Guid TicketId { get; set; }
        public Guid FoodId { get; set; }

        public Ticket Ticket { get; set; }
        public Food Food { get; set; }
    }
}
