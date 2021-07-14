using CinemaProject.BLL.Mappers;
using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace CinemaProject.BLL.Services
{
    public class ActorService
    {
        private readonly UnitOfWork _unitOfWork;

        public ActorService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Actor[] GetAll()
        {
            IQueryable<ActorEntity> actorEntities = _unitOfWork.ActorsRepository.GetAll();

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
            IQueryable<CastEntity> castQuery = _unitOfWork.CastsRepository.GetAll();

            castQuery
                .Where(cast => cast.ActorId == id)
                .Delete();

            await _unitOfWork.ActorsRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
