using System;

namespace CinemaProject.DAL.Entities
{
    public class TicketSeat
    {
        public Guid Id { get; set; }
        public Guid TicketId { get; set; }
        public Guid SeatId { get; set; }

        public Ticket Ticket { get; set; }
        public Seat Seat { get; set; }
    }
}
