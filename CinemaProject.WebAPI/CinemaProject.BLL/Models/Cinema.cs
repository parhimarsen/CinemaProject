using System;

namespace CinemaProject.BLL.Models
{
    public class Cinema
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public City City { get; set; }
    }
}
