using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class Film
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime Year { get; set; }
        public TimeSpan Duration { get; set; }
        public string Director { get; set; }

        public IList<FilmGenre> Genres { get; set; }
        public IList<Cast> Actors { get; set; }
        public IList<Session> Sessions { get; set; }
    }
}
