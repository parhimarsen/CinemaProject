using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("Session")]
    public class SessionEntity
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public Guid FilmId { get; set; }
        public decimal Cost { get; set; }
        public DateTime ShowStart { get; set; }
        public DateTime ShowEnd { get; set; }

        public HallEntity Hall { get; set; }
        public FilmEntity Film { get; set; }

        public IList<TicketEntity> Tickets { get; set; }
    }
}
