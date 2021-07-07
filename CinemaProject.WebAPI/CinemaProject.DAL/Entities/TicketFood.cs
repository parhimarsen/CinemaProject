using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class TicketFood
    {
        [Key]
        public Guid TicketId { get; set; }
        [Key]
        public Guid FoodId { get; set; }

        public Ticket Ticket { get; set; }
        public Food Food { get; set; }
    }
}
