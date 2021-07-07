using System;
using System.Data;
using System.Collections.Generic;

namespace CinemaProject.DAL.Entities
{
    public class Arrangement
    {
        public Guid Id { get; set; }
        public Guid HallId { get; set; }
        public DataTable Schema { get; set; }

        public IList<Seat> Seats { get; set; }
    }
}
