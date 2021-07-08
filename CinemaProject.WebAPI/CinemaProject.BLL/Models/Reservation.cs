using System;

namespace CinemaProject.BLL.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Ticket Ticket { get; set; }
        public Seat Seat { get; set; }
    }
}
