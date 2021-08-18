using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("SessionAmenity")]
    public class SessionAmenityEntity
    {
        public Guid SessionId { get; set; }
        public Guid AmenityId { get; set; }
        public int ExtraPaymentPercent { get; set; }

        public SessionEntity Session { get; set; }
        public AmenityEntity Amenity { get; set; }
    }
}
