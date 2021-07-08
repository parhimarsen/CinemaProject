using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("Film")]
    public class FilmEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime Year { get; set; }
        public TimeSpan Duration { get; set; }
        public string Director { get; set; }

        public IList<FilmGenreEntity> Genres { get; set; }
        public IList<CastEntity> Actors { get; set; }
        public IList<SessionEntity> Sessions { get; set; }
    }
}
