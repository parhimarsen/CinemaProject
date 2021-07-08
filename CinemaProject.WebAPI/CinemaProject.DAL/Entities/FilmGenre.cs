using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class FilmGenre
    {
        public Guid FilmId { get; set; }
        public Guid GenreId { get; set; }

        public Film Film { get; set; }
        public Genre Genre { get; set; }
    }
}
