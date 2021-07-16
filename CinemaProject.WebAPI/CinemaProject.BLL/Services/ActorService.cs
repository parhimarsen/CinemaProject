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

        public IQueryable<Actor> GetAll()
        {
            return _unitOfWork.ActorsRepository.GetAll()
                .Select(actor => actor.ToModel());
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

        public async Task UpdateAsync(Actor actor)
        {
            if (!await _unitOfWork.ActorsRepository.ExistsAsync(actor.Id))
            {
                return;
            }

            ActorEntity actorEntity = await _unitOfWork.ActorsRepository.GetAsync(actor.Id);

            actor.Name = actor.Name;

            await _unitOfWork.ActorsRepository.UpdateAsync(actorEntity.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
