using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class City
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IList<Cinema> Cinemas { get; set; }
    }
}
