using System;
using System.Threading.Tasks;
using CinemaProject.DAL.Contexts;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Interfaces;

namespace CinemaProject.DAL.Repositories
{
    public class UnitOfWork : IDisposable
    {
        public IGenericRepository<UserEntity> Users => new GenericRepository<UserEntity>(_context);
        public IGenericRepository<RefreshTokenEntity> RefreshTokens => new GenericRepository<RefreshTokenEntity>(_context);
        public IGenericRepository<TicketEntity> Tickets => new GenericRepository<TicketEntity>(_context);
        public IGenericRepository<FoodEntity> Food => new GenericRepository<FoodEntity>(_context);
        public IGenericRepository<TicketFoodEntity> TicketFood => new GenericRepository<TicketFoodEntity>(_context);
        public IGenericRepository<FilmEntity> Films => new GenericRepository<FilmEntity>(_context);
        public IGenericRepository<CastEntity> Cast => new GenericRepository<CastEntity>(_context);
        public IGenericRepository<ActorEntity> Actors => new GenericRepository<ActorEntity>(_context);
        public IGenericRepository<GenreEntity> Genres => new GenericRepository<GenreEntity>(_context);
        public IGenericRepository<FilmGenreEntity> FilmGenres => new GenericRepository<FilmGenreEntity>(_context);

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
