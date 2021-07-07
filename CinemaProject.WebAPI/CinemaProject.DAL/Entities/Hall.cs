﻿using System;
using System.Collections.Generic;

namespace CinemaProject.DAL.Entities
{
    public class Hall
    {
        public Guid Id { get; set; }
        public Guid CinemaId { get; set; }
        public string Name { get; set; }
        public int NumberOfSeats { get; set; }

        public Cinema Cinema { get; set; }

        public IList<Session> Sessions { get; set; }
        public Arrangement Arrangement { get; set; }
    }
}
