﻿using System;
using System.Collections.Generic;

namespace CinemaProject.DAL.Entities
{
    public class Food
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public Guid TicketId { get; set; }

        public IList<TicketFood> Tickets { get; set; }
    }
}
