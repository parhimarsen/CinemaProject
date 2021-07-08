using System;

namespace CinemaProject.BLL.Models
{
    public class TypeOfSeatDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ExtraPaymentPercent { get; set; }
    }
}
