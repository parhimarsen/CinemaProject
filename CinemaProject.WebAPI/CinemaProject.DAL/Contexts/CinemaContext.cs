using CinemaProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaProject.DAL.Contexts
{
    public class CinemaContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketFood> TicketFood { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Cast> Cast { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<FilmGenre> FilmGenres { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<TypeOfSeat> TypeOfSeats { get; set; }
        public DbSet<TicketSeat> Reservation { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>()
                .HasOne(e => e.User)
                .WithMany(e => e.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(e => e.User)
                .WithMany(e => e.Tickets)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketFood>().HasKey(e => new { e.TicketId, e.FoodId });

            modelBuilder.Entity<TicketFood>()
                .HasOne(e => e.Food)
                .WithMany(e => e.Tickets)
                .HasForeignKey(e => e.FoodId);

            modelBuilder.Entity<TicketFood>()
                .HasOne(e => e.Ticket)
                .WithMany(e => e.Food)
                .HasForeignKey(e => e.TicketId);

            modelBuilder.Entity<Ticket>()
                .HasOne(e => e.Session)
                .WithMany(e => e.Tickets)
                .HasForeignKey(e => e.SessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Session>()
                .HasOne(e => e.Film)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.FilmId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Session>()
                .HasOne(e => e.Hall)
                .WithMany(e => e.Sessions)
                .HasForeignKey(e => e.HallId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FilmGenre>().HasKey(e => new { e.FilmId, e.GenreId });

            modelBuilder.Entity<FilmGenre>()
                .HasOne(e => e.Film)
                .WithMany(e => e.Genres)
                .HasForeignKey(e => e.FilmId);

            modelBuilder.Entity<FilmGenre>()
                .HasOne(e => e.Genre)
                .WithMany(e => e.Films)
                .HasForeignKey(e => e.GenreId);

            modelBuilder.Entity<Cast>().HasKey(e => new { e.FilmId, e.ActorId });

            modelBuilder.Entity<Cast>()
                .HasOne(e => e.Film)
                .WithMany(e => e.Actors)
                .HasForeignKey(e => e.FilmId);

            modelBuilder.Entity<Cast>()
                .HasOne(e => e.Actor)
                .WithMany(e => e.Films)
                .HasForeignKey(e => e.ActorId);

            modelBuilder.Entity<Hall>()
                .HasOne(e => e.Cinema)
                .WithMany(e => e.Halls)
                .HasForeignKey(e => e.CinemaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cinema>()
                .HasOne(e => e.City)
                .WithMany(e => e.Cinemas)
                .HasForeignKey(e => e.CityId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Seat>()
                .HasOne(e => e.Hall)
                .WithMany(e => e.Seats)
                .HasForeignKey(e => e.HallId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Seat>()
                .HasOne(e => e.TypeOfSeat)
                .WithMany(e => e.Seats)
                .HasForeignKey(e => e.TypeOfSeatId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TicketSeat>().HasKey(e => new { e.TicketId, e.SeatId });

            modelBuilder.Entity<TicketSeat>()
                .HasOne(e => e.Ticket)
                .WithMany(e => e.Seats)
                .HasForeignKey(e => e.TicketId);

            modelBuilder.Entity<TicketSeat>()
                .HasOne(e => e.Seat)
                .WithMany(e => e.SeatReservations)
                .HasForeignKey(e => e.SeatId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connection = @"Data Source=WSA-105-74;Initial Catalog=CinemaDB;Integrated Security=SSPI;";
            optionsBuilder.UseSqlServer(connection);
        }
    }
}
