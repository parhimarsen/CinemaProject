using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public IList<RefreshToken> RefreshTokens { get; set; }
        public IList<Ticket> Tickets { get; set; }

        public User()
        {
            RefreshTokens = new List<RefreshToken>();
        }
    }
}
