using CinemaProject.DAL.Contexts;
using CinemaProject.DAL.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.DAL.Repositories
{
    public class SessionAmenitiesRepository
    {
        private readonly CinemaContext _context;

        public SessionAmenitiesRepository(CinemaContext context)
        {
            _context = context;
        }

        public IQueryable<SessionAmenityEntity> GetAll()
        {
            return _context.Set<SessionAmenityEntity>();
        }

        public async Task<SessionAmenityEntity> GetAsync(Guid sessionId, Guid amenityId)
        {
            return await _context.Set<SessionAmenityEntity>().FindAsync(sessionId, amenityId);
        }

        public async Task<SessionAmenityEntity> InsertAsync(SessionAmenityEntity sessionAmenityEntity)
        {
            await _context.Set<SessionAmenityEntity>().AddAsync(sessionAmenityEntity);

            return sessionAmenityEntity;
        }

        public async Task RemoveAsync(Guid sessionId, Guid amenityId)
        {
            SessionAmenityEntity sessionAmenityEntity = await GetAsync(sessionId, amenityId);

            _context.Set<SessionAmenityEntity>().Remove(sessionAmenityEntity);
        }

        public async Task<bool> ExistsAsync(Guid sessionId, Guid amenityId)
        {
            SessionAmenityEntity sessionAmenityEntity = await GetAsync(sessionId, amenityId);

            if (sessionAmenityEntity == null)
            {
                return false;
            }

            return true;
        }
    }
}
