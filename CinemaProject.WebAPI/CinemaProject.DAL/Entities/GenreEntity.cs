using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("Genre")]
    public class GenreEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public IList<FilmGenreEntity> Films { get; set; }
    }
}
