using System;

namespace CinemaProject.BLL.Models
{
    public class FilmDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime Year { get; set; }
        public TimeSpan Duration { get; set; }
        public string Director { get; set; }

        public ActorDTO[] Actors { get; set; }
        public GenreDTO[] Genres { get; set; }
    }
}
