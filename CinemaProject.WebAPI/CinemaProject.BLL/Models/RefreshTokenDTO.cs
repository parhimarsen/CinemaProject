﻿using System;

namespace CinemaProject.BLL.Models
{
    public class RefreshTokenDTO
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;

        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }

        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }

        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

        public UserDTO User { get; set; }
    }
}
