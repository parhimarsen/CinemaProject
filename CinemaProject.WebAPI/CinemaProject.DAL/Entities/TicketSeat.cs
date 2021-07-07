﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class TicketSeat
    {
        [Key]
        public Guid TicketId { get; set; }
        [Key]
        public Guid SeatId { get; set; }

        public Ticket Ticket { get; set; }
        public Seat Seat { get; set; }
    }
}