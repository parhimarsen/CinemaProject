using CinemaProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaProject.DAL.Contexts
{
    public class CinemaContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<TicketEntity> Tickets { get; set; }
        public DbSet<TicketFoodEntity> TicketFood { get; set; }
        public DbSet<FoodEntity> Food { get; set; }
        public DbSet<SessionEntity> Sessions { get; set; }
        public DbSet<FilmEntity> Films { get; set; }
        public DbSet<CastEntity> Cast { get; set; }
        public DbSet<ActorEntity> Actors { get; set; }
        public DbSet<FilmGenreEntity> FilmGenres { get; set; }
        public DbSet<GenreEntity> Genres { get; set; }
        public DbSet<HallEntity> Halls { get; set; }
        public DbSet<CinemaEntity> Cinemas { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<SeatEntity> Seats { get; set; }
        public DbSet<TypeOfSeatEntity> TypeOfSeats { get; set; }
        public DbSet<TicketSeatEntity> Reservation { get; set; }

        public CinemaContext(DbContextOptions<CinemaContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshTokenEntity>()
                .HasOne(e => e.User)
                .WithMany(e => e.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketEntity>()
                .HasOne(e => e.User)
                .WithMany(e => e.Tickets)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketFoodEntity>().HasKey(e => new { e.TicketId, e.FoodId });

            modelBuilder.Entity<TicketFoodEntity>()
                .HasOne(e => e.Food)
                .WithMany(e => e.Tickets)
                .HasForeignKey(e => e.FoodId);

            modelBuilder.Entity<TicketFoodEntity>()
                .HasOne(e => e.Ticket)
                .WithMany(e => e.Food)
                .HasForeignKey(e => e.TicketId);

            modelBuilder.Entity<TicketEntity>()
                .HasOne(e => e.Session)
                .WithMany(e => e.Tickets)
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SessionEntity>()
                .HasOne(e => e.Film)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.FilmId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SessionEntity>()
                .HasOne(e => e.Hall)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.HallId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FilmGenreEntity>().HasKey(e => new { e.FilmId, e.GenreId });

            modelBuilder.Entity<FilmGenreEntity>()
                .HasOne(e => e.Film)
                .WithMany(e => e.Genres)
                .HasForeignKey(e => e.FilmId);

            modelBuilder.Entity<FilmGenreEntity>()
                .HasOne(e => e.Genre)
                .WithMany(e => e.Films)
                .HasForeignKey(e => e.GenreId);

            modelBuilder.Entity<CastEntity>().HasKey(e => new { e.FilmId, e.ActorId });

            modelBuilder.Entity<CastEntity>()
                .HasOne(e => e.Film)
                .WithMany(e => e.Actors)
                .HasForeignKey(e => e.FilmId);

            modelBuilder.Entity<CastEntity>()
                .HasOne(e => e.Actor)
                .WithMany(e => e.Films)
                .HasForeignKey(e => e.ActorId);

            modelBuilder.Entity<HallEntity>()
                .HasOne(e => e.Cinema)
                .WithMany(e => e.Halls)
                .HasForeignKey(e => e.CinemaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CinemaEntity>()
                .HasOne(e => e.City)
                .WithMany(e => e.Cinemas)
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<SeatEntity>()
                .HasOne(e => e.Hall)
                .WithMany(e => e.Seats)
                .HasForeignKey(e => e.HallId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SeatEntity>()
                .HasOne(e => e.TypeOfSeat)
                .WithMany(e => e.Seats)
                .HasForeignKey(e => e.TypeOfSeatId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TicketSeatEntity>().HasKey(e => new { e.TicketId, e.SeatId });

            modelBuilder.Entity<TicketSeatEntity>()
                .HasOne(e => e.Ticket)
                .WithMany(e => e.Seats)
                .HasForeignKey(e => e.TicketId);

            modelBuilder.Entity<TicketSeatEntity>()
                .HasOne(e => e.Seat)
                .WithMany(e => e.SeatReservations)
                .HasForeignKey(e => e.SeatId);
        }
    }
}
