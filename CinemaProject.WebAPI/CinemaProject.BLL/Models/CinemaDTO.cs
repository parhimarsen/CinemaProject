using System;

namespace CinemaProject.BLL.Models
{
    public class CinemaDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public CityDTO City { get; set; }
    }
}
