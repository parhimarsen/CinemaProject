using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("Seat")]
    public class SeatEntity
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public Guid TypeOfSeatId { get; set; }
        public int NumberOfSeat { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public TypeOfSeatEntity TypeOfSeat { get; set; }
        public HallEntity Hall { get; set; }

        public IList<TicketSeatEntity> SeatReservations { get; set; }
    }
}
