using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class TypeOfSeat
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ExtraPaymentPercent { get; set; }

        public IList<Seat> Seats { get; set; }
    }
}
