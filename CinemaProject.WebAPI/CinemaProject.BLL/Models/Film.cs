using System;
using System.Collections.Generic;

namespace CinemaProject.BLL.Models
{
    public class Film
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime ReleaseDate { get; set; }
        public TimeSpan Duration { get; set; }
        public string Director { get; set; }

        public IList<Actor> Actors { get; set; }
        public IList<Genre> Genres { get; set; }
    }
}
