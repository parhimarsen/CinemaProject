using System;

namespace CinemaProject.BLL.Models
{
    public class TypeOfSeat
    {
        public Guid Id { get; set; }
        public Guid CinemaId { get; set; }
        public string Name { get; set; }
        public int ExtraPaymentPercent { get; set; }
    }
}
