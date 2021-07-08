using System;

namespace CinemaProject.BLL.Models
{
    public class SessionDTO
    {
        public Guid Id { get; set; }
        public decimal Cost { get; set; }
        public DateTime ShowStart { get; set; }
        public DateTime ShowEnd { get; set; }

        public HallDTO Hall { get; set; }
        public FilmDTO Film { get; set; }
    }
}
