using System;

namespace CinemaProject.BLL.Models
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }

        public Session Session { get; set; }
    }
}
