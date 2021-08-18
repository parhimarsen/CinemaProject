using System;

namespace CinemaProject.BLL.Models
{
    public class Amenity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int ExtraPaymentPercent { get; set; }
    }
}
