using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("TicketFood")]
    public class TicketFoodEntity
    {
        public Guid TicketId { get; set; }
        public Guid FoodId { get; set; }

        public TicketEntity Ticket { get; set; }
        public FoodEntity Food { get; set; }
    }
}
