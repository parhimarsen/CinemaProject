using CinemaProject.DAL.Entities;
using System;

namespace CinemaProject.BLL.Models
{
    public class HallDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Cinema Cinema { get; set; }
    }
}
