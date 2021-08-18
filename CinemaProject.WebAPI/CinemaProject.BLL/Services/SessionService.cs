using CinemaProject.BLL.Mappers;
using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.BLL.Services
{
    public class SessionService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly AmenityService _amenityService;

        public SessionService(UnitOfWork unitOfWork, AmenityService amenityService)
        {
            _unitOfWork = unitOfWork;
            _amenityService = amenityService;
        }

        public IQueryable<Session> GetAll()
        {
            return _unitOfWork.SessionsRepository.GetAll()
               .Select(session => new Session
               {
                   Id = session.Id,
                   CinemaName = session.CinemaName,
                   FilmName = session.FilmName,
                   HallName = session.HallName,
                   ShowStart = session.ShowStart,
                   ShowEnd = session.ShowEnd,
                   Cost = session.Cost,
                   HallId = session.HallId,
                   FilmId = session.FilmId
               });
        }

        public async Task<Session> GetAsync(Guid sessionId)
        {
            SessionEntity sessionEntity = await _unitOfWork.SessionsRepository.GetAsync(sessionId);

            if (sessionEntity == null)
            {
                return null;
            }

            return sessionEntity.ToModel();
        }

        public async Task<Session> InsertAsync(Session session)
        {
            if (!await _unitOfWork.FilmsRepository.ExistsAsync(session.FilmId)
                || !await _unitOfWork.HallsRepository.ExistsAsync(session.HallId))
            {
                return null;
            }

            FilmEntity filmEntity = await _unitOfWork.FilmsRepository.GetAsync(session.FilmId);

            SessionEntity newSessionEntity = new SessionEntity
            {
                Id = Guid.NewGuid(),
                FilmName = session.FilmName,
                CinemaName = session.CinemaName,
                HallName = session.HallName,
                ShowStart = session.ShowStart,
                ShowEnd = session.ShowStart.Add(filmEntity.Duration),
                Cost = session.Cost,
                FilmId = session.FilmId,
                HallId = session.HallId,
            };

            await _unitOfWork.SessionsRepository.InsertAsync(newSessionEntity);
            await _unitOfWork.SaveAsync();

            if (session.AffordableAmenities != null)
            {
                foreach (Amenity amenity in session.AffordableAmenities)
                {
                    await _amenityService.InsertToSessionAsync(newSessionEntity.Id, amenity);
                }
            }

            return newSessionEntity.ToModel();
        }

        public async Task RemoveAsync(Guid id)
        {
            if (!await _unitOfWork.SessionsRepository.ExistsAsync(id))
            {
                return;
            }

            await _unitOfWork.SessionsRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Session session)
        {
            if (!await _unitOfWork.SessionsRepository.ExistsAsync(session.Id))
            {
                return;
            }

            SessionEntity sessionEntity = await _unitOfWork.SessionsRepository.GetAsync(session.Id);

            FilmEntity filmEntity = await _unitOfWork.FilmsRepository.GetAsync(session.FilmId);

            sessionEntity.ShowStart = session.ShowStart;
            sessionEntity.ShowEnd = session.ShowStart.Add(filmEntity.Duration);
            sessionEntity.HallId = session.HallId;
            sessionEntity.FilmId = session.FilmId;
            sessionEntity.Cost = session.Cost;
            sessionEntity.FilmName = session.FilmName;
            sessionEntity.CinemaName = session.CinemaName;
            sessionEntity.HallName = session.HallName;

            await _unitOfWork.SessionsRepository.UpdateAsync(sessionEntity.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
