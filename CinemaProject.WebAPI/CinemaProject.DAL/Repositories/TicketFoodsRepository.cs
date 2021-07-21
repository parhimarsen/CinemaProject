using CinemaProject.DAL.Contexts;
using CinemaProject.DAL.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.DAL.Repositories
{
    public class TicketFoodsRepository
    {
        private readonly CinemaContext _context;

        public TicketFoodsRepository(CinemaContext context)
        {
            _context = context;
        }

        public IQueryable<TicketFoodEntity> GetAll()
        {
            return _context.Set<TicketFoodEntity>();
        }

        public async Task<TicketFoodEntity> GetAsync(Guid ticketId, Guid foodId)
        {
            return await _context.Set<TicketFoodEntity>().FindAsync(ticketId, foodId);
        }

        public async Task<TicketFoodEntity> InsertAsync(TicketFoodEntity ticketFoodEntity)
        {
            await _context.Set<TicketFoodEntity>().AddAsync(ticketFoodEntity);

            return ticketFoodEntity;
        }

        public async Task RemoveAsync(Guid ticketId, Guid foodId)
        {
            TicketFoodEntity ticketFoodEntity = await GetAsync(ticketId, foodId);

            _context.Set<TicketFoodEntity>().Remove(ticketFoodEntity);
        }

        public async Task<bool> ExistsAsync(Guid ticketId, Guid foodId)
        {
            TicketFoodEntity ticketFoodEntity = await GetAsync(ticketId, foodId);

            if (ticketFoodEntity == null)
            {
                return false;
            }

            return true;
        }
    }
}
