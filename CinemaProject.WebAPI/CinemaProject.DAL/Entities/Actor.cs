using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class Actor
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IList<Cast> Films { get; set; }
    }
}
