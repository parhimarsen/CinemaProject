using System;

namespace CinemaProject.BLL.Models
{
    public class ReservationDTO
    {
        public Guid Id { get; set; }
        public TicketDTO Ticket { get; set; }
        public SeatDTO Seat { get; set; }
    }
}
