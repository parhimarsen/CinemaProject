using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("Hall")]
    public class HallEntity
    {
        public Guid Id { get; set; }
        public Guid CinemaId { get; set; }
        public string Name { get; set; }

        public CinemaEntity Cinema { get; set; }

        public IList<SessionEntity> Sessions { get; set; }
        public IList<SeatEntity> Seats { get; set; }
    }
}
