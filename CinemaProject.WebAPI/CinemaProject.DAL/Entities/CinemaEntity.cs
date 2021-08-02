using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("Cinema")]
    public class CinemaEntity
    {
        public Guid Id { get; set; }
        public Guid? CityId { get; set; }
        public string Name { get; set; }

#nullable enable
        public CityEntity? City { get; set; }
#nullable enable
        public IList<HallEntity>? Halls { get; set; }
    }
}
