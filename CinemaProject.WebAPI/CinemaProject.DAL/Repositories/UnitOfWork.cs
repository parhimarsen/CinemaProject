using System;
using System.Threading.Tasks;
using CinemaProject.DAL.Contexts;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Interfaces;

namespace CinemaProject.DAL.Repositories
{
    public class UnitOfWork : IDisposable
    {
        public IGenericRepository<User> Users => new GenericRepository<User>(_context);
        public IGenericRepository<RefreshToken> RefreshTokens => new GenericRepository<RefreshToken>(_context);
        public IGenericRepository<Ticket> Tickets => new GenericRepository<Ticket>(_context);
        public IGenericRepository<Food> Food => new GenericRepository<Food>(_context);
        public IGenericRepository<TicketFood> TicketFood => new GenericRepository<TicketFood>(_context);
        public IGenericRepository<Film> Films => new GenericRepository<Film>(_context);
        public IGenericRepository<Cast> Cast => new GenericRepository<Cast>(_context);
        public IGenericRepository<Actor> Actors => new GenericRepository<Actor>(_context);
        public IGenericRepository<Genre> Genres => new GenericRepository<Genre>(_context);
        public IGenericRepository<FilmGenre> FilmGenres => new GenericRepository<FilmGenre>(_context);

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
            GC.SuppressFinalize(this);
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
