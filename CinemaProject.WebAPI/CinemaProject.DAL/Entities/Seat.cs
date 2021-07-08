using System;
using System.Collections.Generic;

namespace CinemaProject.DAL.Entities
{
    public class Seat
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public Guid TypeOfSeatId { get; set; }
        public int NumberOfSeat { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public TypeOfSeat TypeOfSeat { get; set; }
        public Hall Hall { get; set; }

        public IList<TicketSeat> SeatReservations { get; set; }
    }
}
