using System;

namespace CinemaProject.BLL.Models
{
    public class Film
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime Year { get; set; }
        public TimeSpan Duration { get; set; }
        public string Director { get; set; }

        public Actor[] Actors { get; set; }
        public Genre[] Genres { get; set; }
    }
}
