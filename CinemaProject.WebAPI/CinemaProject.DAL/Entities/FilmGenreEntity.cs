using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("FilmGenre")]
    public class FilmGenreEntity
    {
        public Guid FilmId { get; set; }
        public Guid GenreId { get; set; }

        public FilmEntity Film { get; set; }
        public GenreEntity Genre { get; set; }
    }
}
