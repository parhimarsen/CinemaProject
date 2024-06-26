﻿using System;

namespace CinemaProject.BLL.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public Ticket[] Tickets { get; set; }
    }
}
