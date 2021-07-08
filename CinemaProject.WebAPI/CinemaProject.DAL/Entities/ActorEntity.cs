using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("Actor")]
    public class ActorEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IList<CastEntity> Films { get; set; }
    }
}
