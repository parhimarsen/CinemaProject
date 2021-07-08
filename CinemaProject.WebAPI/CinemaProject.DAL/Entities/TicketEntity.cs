using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("Ticket")]
    public class TicketEntity
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public bool Status { get; set; }

        public UserEntity User { get; set; }
        public SessionEntity Session { get; set; }

        public IList<TicketSeatEntity> Seats { get; set; }
        public IList<TicketFoodEntity> Food { get; set; }
    }
}
