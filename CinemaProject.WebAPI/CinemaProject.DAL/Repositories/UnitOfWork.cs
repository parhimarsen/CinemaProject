using System;
using System.Threading.Tasks;
using CinemaProject.DAL.Contexts;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Interfaces;

namespace CinemaProject.DAL.Repositories
{
    public class UnitOfWork : IDisposable
    {
        public UsersRepository UsersRepository => new UsersRepository(_context);
        public IGenericRepository<RefreshTokenEntity> RefreshTokensRepository => new GenericRepository<RefreshTokenEntity>(_context);
        public IGenericRepository<TicketEntity> TicketsRepository => new GenericRepository<TicketEntity>(_context);
        public IGenericRepository<AmenityEntity> AmenitiesRepository => new GenericRepository<AmenityEntity>(_context);
        public TicketAmenitiesRepository TicketAmenitiesRepository => new TicketAmenitiesRepository(_context);
        public SessionAmenitiesRepository SessionAmenitiesRepository => new SessionAmenitiesRepository(_context);
        public IGenericRepository<FilmEntity> FilmsRepository => new GenericRepository<FilmEntity>(_context);
        public CastsRepository CastsRepository => new CastsRepository(_context);
        public IGenericRepository<ActorEntity> ActorsRepository => new GenericRepository<ActorEntity>(_context);
        public IGenericRepository<GenreEntity> GenresRepository => new GenericRepository<GenreEntity>(_context);
        public FilmGenresRepository FilmGenresRepository => new FilmGenresRepository(_context);
        public IGenericRepository<SessionEntity> SessionsRepository => new GenericRepository<SessionEntity>(_context);
        public IGenericRepository<HallEntity> HallsRepository => new GenericRepository<HallEntity>(_context);
        public IGenericRepository<CinemaEntity> CinemasRepository => new GenericRepository<CinemaEntity>(_context);
        public IGenericRepository<SeatEntity> SeatsRepository => new GenericRepository<SeatEntity>(_context);
        public IGenericRepository<TypeOfSeatEntity> TypesOfSeatRepository => new GenericRepository<TypeOfSeatEntity>(_context);
        public IGenericRepository<CityEntity> CitiesRepository => new GenericRepository<CityEntity>(_context);
        public TicketSeatsRepository TicketSeatsRepository => new TicketSeatsRepository(_context);

        private readonly CinemaContext _context;
        private bool disposed = false;

        public UnitOfWork(CinemaContext context)
        {
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposed = true;
            }
        }
    }
}
