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
    public class ActorService
    {
        private readonly UnitOfWork _unitOfWork;

        public ActorService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Actor> GetAll()
        {
            return _unitOfWork.ActorsRepository
                .GetAll()
                .Select(actor => new Actor
                {
                    Id = actor.Id,
                    Name = actor.Name
                });
        }

        public async Task<Actor[]> GetAllOfFilm(Guid filmId)
        {
            if (!await _unitOfWork.FilmsRepository.ExistsAsync(filmId))
            {
                return null;
            }

            IList<Actor> actorsOfFilm = new List<Actor>();

            IQueryable<CastEntity> castQuery = _unitOfWork.CastsRepository.GetAll();

            IEnumerable<ActorEntity> actors = castQuery
                .Where(x => x.FilmId == filmId)
                .Select(x => x.Actor);

            foreach (var actor in actors)
            {
                actorsOfFilm.Add(actor.ToModel());
            }

            return actorsOfFilm.ToArray();
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

        public async Task<Actor[]> InsertToFilmAsync(Guid filmId, Actor[] actors)
        {
            if (!await _unitOfWork.FilmsRepository.ExistsAsync(filmId))
            {
                return null;
            }

            List<Actor> existingActors = new List<Actor>();

            foreach (var actor in actors)
            {
                if (!await _unitOfWork.ActorsRepository.ExistsAsync(actor.Id))
                {
                    break;
                }

                if (!await _unitOfWork.CastsRepository.ExistsAsync(filmId, actor.Id))
                {
                    CastEntity castEntity = new CastEntity
                    {
                        ActorId = actor.Id,
                        FilmId = filmId
                    };

                    await _unitOfWork.CastsRepository.InsertAsync(castEntity);
                    await _unitOfWork.SaveAsync();
                }

                existingActors.Add(actor);
            }

            return existingActors.ToArray();
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

            actorEntity.Name = actor.Name;

            await _unitOfWork.ActorsRepository.UpdateAsync(actorEntity.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
