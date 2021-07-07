using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class FilmGenre
    {
        [Key]
        public Guid FilmId { get; set; }
        [Key]
        public Guid GenreId { get; set; }

        public Film Film { get; set; }
        public Genre Genre { get; set; }
    }
}
