using CinemaProject.DAL.Contexts;
using CinemaProject.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.DAL.Repositories
{
    public class CastsRepository
    {
        private readonly CinemaContext _context;

        public CastsRepository(CinemaContext context)
        {
            _context = context;
        }

        public IQueryable<CastEntity> GetAll()
        {
            return _context.Set<CastEntity>();
        }

        public async Task<CastEntity> GetAsync(Guid filmId, Guid actorId)
        {
            return await _context.Set<CastEntity>().FindAsync(filmId, actorId);
        }

        public async Task<CastEntity> InsertAsync(CastEntity castEntity)
        {
            await _context.Set<CastEntity>().AddAsync(castEntity);

            return castEntity;
        }

        public async Task RemoveAsync(Guid filmId, Guid actorId)
        {
            CastEntity castEntity = await GetAsync(filmId, actorId);

            _context.Set<CastEntity>().Remove(castEntity);
        }

        public async Task<bool> ExistsAsync(Guid filmId, Guid actorId)
        {
            CastEntity castEntity = await GetAsync(filmId, actorId);

            if (castEntity == null)
            {
                return false;
            }

            return true;
        }
    }
}
