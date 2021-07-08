using System;

namespace CinemaProject.BLL.Models
{
    public class Seat
    {
        public Guid Id { get; set; }
        public int NumberOfSeat { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public TypeOfSeat TypeOfSeat { get; set; }
    }
}
