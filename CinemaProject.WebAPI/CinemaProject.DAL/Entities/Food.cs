using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class Food
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public Guid TicketId { get; set; }

        public IList<TicketFood> Tickets { get; set; }
    }
}
