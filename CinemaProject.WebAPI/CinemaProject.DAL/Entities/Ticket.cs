using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public bool Status { get; set; }

        public User User { get; set; }
        public Session Session { get; set; }

        public IList<TicketSeat> Seats { get; set; }
        public IList<TicketFood> Food { get; set; }
    }
}
