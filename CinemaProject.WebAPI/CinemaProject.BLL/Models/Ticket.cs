using System;

namespace CinemaProject.BLL.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public Guid SeatId { get; set; }

        public Amenity[] Amenities { get; set; }
    }
}
