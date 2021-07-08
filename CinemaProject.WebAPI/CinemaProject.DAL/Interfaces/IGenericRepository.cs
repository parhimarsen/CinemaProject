using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CinemaProject.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties);
        Task<T> InsertAsync(T item);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
