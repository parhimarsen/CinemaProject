using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("TicketAmenity")]
    public class TicketAmenityEntity
    {
        public Guid TicketId { get; set; }
        public Guid AmenityId { get; set; }

        public TicketEntity Ticket { get; set; }
        public AmenityEntity Amenity { get; set; }
    }
}
