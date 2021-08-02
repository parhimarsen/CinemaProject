using CinemaProject.DAL.Contexts;
using CinemaProject.DAL.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.DAL.Repositories
{
    public class TicketSeatsRepository
    {
        private readonly CinemaContext _context;

        public TicketSeatsRepository(CinemaContext context)
        {
            _context = context;
        }
        public IQueryable<TicketSeatEntity> GetAll()
        {
            return _context.Set<TicketSeatEntity>();
        }

        public async Task<TicketSeatEntity> GetAsync(Guid ticketId, Guid seatId)
        {
            return await _context.Set<TicketSeatEntity>().FindAsync(ticketId, seatId);
        }

        public async Task<TicketSeatEntity> InsertAsync(TicketSeatEntity ticketSeatEntity)
        {
            await _context.Set<TicketSeatEntity>().AddAsync(ticketSeatEntity);

            return ticketSeatEntity;
        }

        public async Task RemoveAsync(Guid ticketId, Guid seatId)
        {
            TicketSeatEntity ticketSeatEntity = await GetAsync(ticketId, seatId);

            _context.Set<TicketSeatEntity>().Remove(ticketSeatEntity);
        }

        public async Task<bool> ExistsAsync(Guid ticketId, Guid seatId)
        {
            TicketSeatEntity ticketSeatEntity = await GetAsync(ticketId, seatId);

            if (ticketSeatEntity == null)
            {
                return false;
            }

            return true;
        }
    }
}
