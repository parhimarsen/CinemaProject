using System;

namespace CinemaProject.DAL.Entities
{
    public class Cast
    {
        public Guid Id { get; set; }
        public Guid FilmId { get; set; }
        public Guid ActorId { get; set; }

        public Film Film { get; set; }
        public Actor Actor { get; set; }
    }
}
