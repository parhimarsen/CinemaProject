using CinemaProject.BLL.Mappers;
using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Repositories;
using System;
using System.Collections.Generic;
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

        public async Task<Session[]> GetAllAsync()
        {
            IEnumerable<SessionEntity> sessionEntities = await _unitOfWork.SessionsRepository.GetAllAsync();

            return sessionEntities
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
    }
}
