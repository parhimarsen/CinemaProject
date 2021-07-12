using CinemaProject.DAL.Contexts;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CinemaProject.DAL.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly CinemaContext _context;

        public UsersRepository(CinemaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            return await _context.Set<UserEntity>().ToListAsync();
        }

        public async Task<UserEntity> GetAsync(Guid id)
        {
            return await _context.Set<UserEntity>().FindAsync(id);
        }

        public IEnumerable<UserEntity> GetWithInclude(params Expression<Func<UserEntity, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }

        public async Task<UserEntity> InsertAsync(UserEntity userEntity)
        {
            await _context.Set<UserEntity>().AddAsync(userEntity);

            return userEntity;
        }
        public async Task<RefreshTokenEntity> InsertRefreshTokenAsync(RefreshTokenEntity refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);

            return refreshToken;
        }

        public async Task RemoveAsync(Guid id)
        {
            UserEntity userEntity = await GetAsync(id);

            _context.Set<UserEntity>().Remove(userEntity);
        }

        public async Task UpdateAsync(Guid id)
        {
            UserEntity userEntity = await GetAsync(id);

            if (userEntity != null)
            {
                _context.Entry(userEntity).State = EntityState.Detached;
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            UserEntity userEntity = await GetAsync(id);

            if (userEntity == null)
            {
                return false;
            }

            return true;
        }

        private IQueryable<UserEntity> Include(params Expression<Func<UserEntity, object>>[] includeProperties)
        {
            IQueryable<UserEntity> query = _context.Set<UserEntity>();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
