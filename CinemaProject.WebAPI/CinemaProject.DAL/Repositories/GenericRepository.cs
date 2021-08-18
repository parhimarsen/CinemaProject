using CinemaProject.DAL.Contexts;
using CinemaProject.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CinemaProject.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly CinemaContext _context;

        public GenericRepository(CinemaContext context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task<T> GetAsync(Guid? id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public async Task<T> InsertAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);

            return item;
        }

        public async Task RemoveAsync(Guid id)
        {
            T item = await GetAsync(id);

            _context.Set<T>().Remove(item);
        }

        public async Task UpdateAsync(Guid id)
        {
            T item = await GetAsync(id);

            if (item != null)
            {
                _context.Attach(item);
                _context.Entry(item).State = EntityState.Modified;
            }
        }

        public async Task<bool> ExistsAsync(Guid? id)
        {
            T item = await GetAsync(id);

            if (item == null)
            {
                return false;
            }

            return true;
        }
    }
}
