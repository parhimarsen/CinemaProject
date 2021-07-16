using System;

namespace CinemaProject.BLL.Models
{
    public class Hall
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CinemaId { get; set; }

        public Seat[] Seats { get; set; }
    }
}
