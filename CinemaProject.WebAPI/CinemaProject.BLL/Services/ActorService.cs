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
    public class ActorService
    {
        private readonly UnitOfWork _unitOfWork;

        public ActorService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Actor[]> GetAllAsync()
        {
            IEnumerable<ActorEntity> actorEntities = await _unitOfWork.ActorsRepository.GetAllAsync();

            return actorEntities
                .Select(actor => actor.ToModel())
                .ToArray();
        }

        public async Task<Actor> InsertAsync(Actor actor)
        {
            ActorEntity actorEntity = new ActorEntity
            {
                Id = Guid.NewGuid(),
                Name = actor.Name,
            };

            await _unitOfWork.ActorsRepository.InsertAsync(actorEntity);
            await _unitOfWork.SaveAsync();

            return actorEntity.ToModel();
        }

        public async Task RemoveAsync(Guid id)
        {
            IEnumerable<CastEntity> castEntities = await _unitOfWork.CastsRepository.GetAllAsync();

            CastEntity[] casts = castEntities
                .Where(cast => cast.ActorId == id)
                .ToArray();

            foreach (var cast in casts)
            {
                await _unitOfWork.CastsRepository.RemoveAsync(cast.FilmId, cast.ActorId);
            }

            await _unitOfWork.ActorsRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
