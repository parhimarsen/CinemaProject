using CinemaProject.DAL.Entities;
using System.Threading.Tasks;

namespace CinemaProject.DAL.Interfaces
{
    public interface IUsersRepository : IGenericRepository<UserEntity>
    {
        Task<RefreshTokenEntity> InsertRefreshTokenAsync(RefreshTokenEntity refreshToken);
    }
}
