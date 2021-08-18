using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("Amenity")]
    public class AmenityEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }

        public IList<TicketAmenityEntity> Tickets { get; set; }
        public IList<SessionAmenityEntity> Sessions { get; set; }
    }
}
