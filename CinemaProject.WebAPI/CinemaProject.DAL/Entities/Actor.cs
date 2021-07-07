using System;
using System.Collections.Generic;

namespace CinemaProject.DAL.Entities
{
    public class Actor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IList<Film> Films { get; set; }
    }
}
