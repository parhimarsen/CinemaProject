using System;

namespace CinemaProject.BLL.Models
{
    public class Cinema
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CityId { get; set; }

        public Hall[] Halls { get; set; }
    }
}
