using System;

namespace CinemaProject.BLL.Models
{
    public class SeatDTO
    {
        public Guid Id { get; set; }
        public int NumberOfSeat { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public HallDTO Hall { get; set; }
        public TypeOfSeatDTO TypeOfSeat { get; set; }
    }
}
