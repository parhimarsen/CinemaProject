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

        public SessionService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Session[] GetAllAsync()
        {
            IQueryable<SessionEntity> sessionQuery = _unitOfWork.SessionsRepository.GetAll();

            return sessionQuery
                .Select(session => session.ToModel())
                .ToArray();
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
            if (!await _unitOfWork.FilmsRepository.ExistsAsync(session.FilmId) && !await _unitOfWork.HallsRepository.ExistsAsync(session.HallId))
            {
                return null;
            }

            SessionEntity newSessionEntity = new SessionEntity
            {
                Id = Guid.NewGuid(),
                ShowStart = session.ShowStart,
                ShowEnd = session.ShowEnd,
                Cost = session.Cost,
                FilmId = session.FilmId,
                HallId = session.HallId
            };

            await _unitOfWork.SessionsRepository.InsertAsync(newSessionEntity);
            await _unitOfWork.SaveAsync();

            return newSessionEntity.ToModel();
        }

        public async Task RemoveAsync(Guid id)
        {
            if (!await _unitOfWork.SessionsRepository.ExistsAsync(id))
            {
                return;
            }

            SessionEntity sessionEntity = _unitOfWork.SessionsRepository
                .GetWithInclude(session => session.Tickets)
                .FirstOrDefault(ticket => ticket.Id == id);

            FilmEntity filmEntity = await _unitOfWork.FilmsRepository.GetAsync(sessionEntity.FilmId);
            filmEntity.Sessions.Remove(sessionEntity);
            await _unitOfWork.SaveAsync();

            HallEntity hallEntity = await _unitOfWork.HallsRepository.GetAsync(sessionEntity.HallId);
            hallEntity.Sessions.Remove(sessionEntity);
            await _unitOfWork.SaveAsync();

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

            sessionEntity.ShowStart = session.ShowStart;
            sessionEntity.ShowEnd = session.ShowEnd;
            sessionEntity.HallId = session.HallId;
            sessionEntity.FilmId = session.FilmId;
            sessionEntity.Cost = session.Cost;

            await _unitOfWork.SessionsRepository.UpdateAsync(sessionEntity.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
