using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("City")]
    public class CityEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IList<CinemaEntity> Cinemas { get; set; }
    }
}
