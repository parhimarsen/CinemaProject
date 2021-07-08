using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("Cast")]
    public class CastEntity
    {
        public Guid FilmId { get; set; }
        public Guid ActorId { get; set; }

        public FilmEntity Film { get; set; }
        public ActorEntity Actor { get; set; }
    }
}
