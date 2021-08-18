using System;

namespace CinemaProject.BLL.Models
{
    public class Session
    {
        public Guid Id { get; set; }
        public string CinemaName { get; set; }
        public string HallName { get; set; }
        public string FilmName { get; set; }
        public decimal Cost { get; set; }
        public DateTime ShowStart { get; set; }
        public DateTime ShowEnd { get; set; }
        public Guid? HallId { get; set; }
        public Guid? FilmId { get; set; } 

        public Amenity[] AffordableAmenities { get; set; }
    }
}
