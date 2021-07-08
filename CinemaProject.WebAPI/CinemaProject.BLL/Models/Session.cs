using System;

namespace CinemaProject.BLL.Models
{
    public class Session
    {
        public Guid Id { get; set; }
        public decimal Cost { get; set; }
        public DateTime ShowStart { get; set; }
        public DateTime ShowEnd { get; set; }

        public Hall Hall { get; set; }
        public Film Film { get; set; }
    }
}
