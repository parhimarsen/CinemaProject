using CinemaProject.DAL.Contexts;
using CinemaProject.DAL.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.DAL.Repositories
{
    public class TicketAmenitiesRepository
    {
        private readonly CinemaContext _context;

        public TicketAmenitiesRepository(CinemaContext context)
        {
            _context = context;
        }

        public IQueryable<TicketAmenityEntity> GetAll()
        {
            return _context.Set<TicketAmenityEntity>();
        }

        public async Task<TicketAmenityEntity> GetAsync(Guid ticketId, Guid amenityId)
        {
            return await _context.Set<TicketAmenityEntity>().FindAsync(ticketId, amenityId);
        }

        public async Task<TicketAmenityEntity> InsertAsync(TicketAmenityEntity ticketAmenityEntity)
        {
            await _context.Set<TicketAmenityEntity>().AddAsync(ticketAmenityEntity);

            return ticketAmenityEntity;
        }

        public async Task RemoveAsync(Guid ticketId, Guid amenityId)
        {
            TicketAmenityEntity ticketAmenityEntity = await GetAsync(ticketId, amenityId);

            _context.Set<TicketAmenityEntity>().Remove(ticketAmenityEntity);
        }

        public async Task<bool> ExistsAsync(Guid ticketId, Guid amenityId)
        {
            TicketAmenityEntity ticketAmenityEntity = await GetAsync(ticketId, amenityId);

            if (ticketAmenityEntity == null)
            {
                return false;
            }

            return true;
        }
    }
}
