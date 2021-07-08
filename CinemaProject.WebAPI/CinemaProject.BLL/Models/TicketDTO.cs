using System;

namespace CinemaProject.BLL.Models
{
    public class TicketDTO
    {
        public Guid Id { get; set; }
        public bool Status { get; set; }

        public UserDTO User { get; set; }
        public SessionDTO Session { get; set; }
    }
}
