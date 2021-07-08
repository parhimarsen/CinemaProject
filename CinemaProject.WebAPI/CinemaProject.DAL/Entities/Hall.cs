using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class Hall
    {
        public Guid Id { get; set; }
        public Guid CinemaId { get; set; }
        public string Name { get; set; }

        public Cinema Cinema { get; set; }

        public IList<Session> Sessions { get; set; }
        public IList<Seat> Seats { get; set; }
    }
}
