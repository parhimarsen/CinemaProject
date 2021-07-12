using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("TicketSeat")]
    public class TicketSeatEntity
    {
        public Guid TicketId { get; set; }
        public Guid SeatId { get; set; }
        public decimal CostWithPercent { get; set; }

        public TicketEntity Ticket { get; set; }
        public SeatEntity Seat { get; set; }
    }
}
