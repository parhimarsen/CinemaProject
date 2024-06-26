﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaProject.DAL.Entities
{
    [Table("RefreshToken")]
    public class RefreshTokenEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public String Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;

        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }

        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }

        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

        public UserEntity User { get; set; }
    }
}
