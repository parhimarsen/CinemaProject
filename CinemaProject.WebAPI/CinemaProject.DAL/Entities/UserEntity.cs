using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("User")]
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public IList<RefreshTokenEntity> RefreshTokens { get; set; }
        public IList<TicketEntity> Tickets { get; set; }

        public UserEntity()
        {
            RefreshTokens = new List<RefreshTokenEntity>();
        }
    }
}
