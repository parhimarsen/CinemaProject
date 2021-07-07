using System;
using System.Collections.Generic;

namespace CinemaProject.DAL.Entities
{
    public class Seat
    {
        public Guid Id { get; set; }
        public Guid ArrangementId { get; set; }
        public Guid TypeId { get; set; }
        public int NumberOfSeat { get; set; }

        public TypeOfSeat TypeOfSeat { get; set; }
        public Arrangement Arrangement { get; set; }

        public IList<TicketSeat> SeatReservation { get; set; }
    }
}
