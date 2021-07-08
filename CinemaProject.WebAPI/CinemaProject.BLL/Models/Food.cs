using System;

namespace CinemaProject.BLL.Models
{
    public class Food
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public Ticket Ticket { get; set; }
    }
}
