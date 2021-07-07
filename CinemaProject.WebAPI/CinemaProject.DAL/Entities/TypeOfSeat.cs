using System;

namespace CinemaProject.DAL.Entities
{
    public class TypeOfSeat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ExtraPaymentPercent { get; set; }
    }
}
