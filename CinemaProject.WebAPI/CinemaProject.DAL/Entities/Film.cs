using System;
using System.Collections.Generic;

namespace CinemaProject.DAL.Entities
{
    public class Film
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Year { get; set; }
        public TimeSpan Duration { get; set; }
        public string Director { get; set; }
        public string Genre { get; set; }

        public IList<Cast> Actors { get; set; }
        public IList<Session> Sessions { get; set; }
    }
}
