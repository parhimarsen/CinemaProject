﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaProject.DAL.Entities
{
    public class Cast
    {
        [Key]
        public Guid FilmId { get; set; }
        [Key]
        public Guid ActorId { get; set; }

        public Film Film { get; set; }
        public Actor Actor { get; set; }
    }
}