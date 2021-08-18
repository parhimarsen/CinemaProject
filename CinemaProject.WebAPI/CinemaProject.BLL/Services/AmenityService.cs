using CinemaProject.BLL.Mappers;
using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace CinemaProject.BLL.Services
{
    public class AmenityService
    {
        private readonly UnitOfWork _unitOfWork;

        public AmenityService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Amenity> GetAll()
        {
            return _unitOfWork.AmenitiesRepository
                .GetAll()
                .Select(amenity => new Amenity
                {
                    Id = amenity.Id,
                    Name = amenity.Name,
                    Cost = amenity.Cost
                });
        }

        public async Task<Amenity[]> GetAllOfTicketAsync(Guid ticketId)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(ticketId))
            {
                return null;
            }

            IList<Amenity> amenitiesOfTicket = new List<Amenity>();

            IQueryable<TicketAmenityEntity> ticketAmenityQuery = _unitOfWork.TicketAmenitiesRepository.GetAll();

            IEnumerable<AmenityEntity> amenities = ticketAmenityQuery
                .Where(x => x.TicketId == ticketId)
                .Select(x => x.Amenity);

            foreach (var amenity in amenities)
            {
                amenitiesOfTicket.Add(amenity.ToModel());
            }

            return amenitiesOfTicket.ToArray();
        }

        public async Task<Amenity[]> GetAllOfSessionAsync(Guid sessionId)
        {
            if (!await _unitOfWork.SessionsRepository.ExistsAsync(sessionId))
            {
                return null;
            }

            IList<Amenity> amenitiesOfSession = new List<Amenity>();

            IQueryable<SessionAmenityEntity> sessionAmenityQuery = _unitOfWork.SessionAmenitiesRepository.GetAll();

            IEnumerable<AmenityEntity> amenities = sessionAmenityQuery
                .Where(x => x.SessionId == sessionId)
                .Select(x => x.Amenity)
                .ToArray();

            foreach (var amenity in amenities)
            {
                SessionAmenityEntity sessionAmenity = await _unitOfWork.SessionAmenitiesRepository.GetAsync(sessionId, amenity.Id);

                Amenity amenityWithPercents = amenity.ToModel();
                amenityWithPercents.ExtraPaymentPercent = sessionAmenity.ExtraPaymentPercent;

                amenitiesOfSession.Add(amenityWithPercents);
            }

            return amenitiesOfSession.ToArray();
        }

        public async Task<Amenity> InsertAsync(Amenity amenity)
        {
            AmenityEntity amenityEntity = new AmenityEntity
            {
                Id = Guid.NewGuid(),
                Name = amenity.Name,
                Cost = amenity.Cost,
            };

            await _unitOfWork.AmenitiesRepository.InsertAsync(amenityEntity);
            await _unitOfWork.SaveAsync();

            return amenityEntity.ToModel();
        }

        public async Task<bool> InsertToTicketAsync(Guid ticketId, Guid amenityId)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(ticketId)
                || !await _unitOfWork.AmenitiesRepository.ExistsAsync(amenityId))
            {
                return false;
            }

            TicketAmenityEntity ticketAmenityEntity = new TicketAmenityEntity
            {
                TicketId = ticketId,
                AmenityId = amenityId
            };

            await _unitOfWork.TicketAmenitiesRepository.InsertAsync(ticketAmenityEntity);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task<bool> InsertToSessionAsync(Guid sessionId, Amenity amenity)
        {
            if (!await _unitOfWork.SessionsRepository.ExistsAsync(sessionId)
                || !await _unitOfWork.AmenitiesRepository.ExistsAsync(amenity.Id))
            {
                return false;
            }

            SessionAmenityEntity sessionAmenityEntity = new SessionAmenityEntity
            {
                SessionId = sessionId,
                AmenityId = amenity.Id,
                ExtraPaymentPercent = amenity.ExtraPaymentPercent
            };

            await _unitOfWork.SessionAmenitiesRepository.InsertAsync(sessionAmenityEntity);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task RemoveAsync(Guid ticketId)
        {
            IQueryable<TicketAmenityEntity> ticketAmenityQuery = _unitOfWork.TicketAmenitiesRepository.GetAll();

            ticketAmenityQuery
                .Where(ticket => ticket.TicketId == ticketId)
                .Delete();


            await _unitOfWork.AmenitiesRepository.RemoveAsync(ticketId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveFromTicketAsync(Guid ticketId, Guid amenityId)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(ticketId) && !await _unitOfWork.TicketsRepository.ExistsAsync(amenityId))
            {
                return;
            }

            await _unitOfWork.TicketAmenitiesRepository.RemoveAsync(ticketId, amenityId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveFromSessionAsync(Guid sessionId, Guid amenityId)
        {
            if (!await _unitOfWork.SessionsRepository.ExistsAsync(sessionId) && !await _unitOfWork.AmenitiesRepository.ExistsAsync(amenityId))
            {
                return;
            }

            await _unitOfWork.SessionAmenitiesRepository.RemoveAsync(sessionId, amenityId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Amenity amenity)
        {
            if (!await _unitOfWork.AmenitiesRepository.ExistsAsync(amenity.Id))
            {
                return;
            }

            AmenityEntity amenityEntity = await _unitOfWork.AmenitiesRepository.GetAsync(amenity.Id);

            amenityEntity.Id = amenity.Id;
            amenityEntity.Name = amenity.Name;
            amenityEntity.Cost = amenity.Cost;

            await _unitOfWork.AmenitiesRepository.UpdateAsync(amenity.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
