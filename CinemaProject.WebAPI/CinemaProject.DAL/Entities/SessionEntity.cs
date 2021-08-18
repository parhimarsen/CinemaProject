using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("Session")]
    public class SessionEntity
    {
        public Guid Id { get; set; }
        public Guid? HallId { get; set; }
        public Guid? FilmId { get; set; }
        public string CinemaName { get; set; }
        public string HallName { get; set; }
        public string FilmName { get; set; }
        public decimal Cost { get; set; }
        public DateTime ShowStart { get; set; }
        public DateTime ShowEnd { get; set; }

#nullable enable
        public HallEntity? Hall { get; set; }
        public FilmEntity? Film { get; set; }
#nullable disable
        public IList<TicketEntity> Tickets { get; set; }
        public IList<SessionAmenityEntity> Amenities { get; set; }
    }
}
