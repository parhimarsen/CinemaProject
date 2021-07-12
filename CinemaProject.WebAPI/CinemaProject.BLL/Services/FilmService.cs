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
    public class FilmService
    {
        private readonly UnitOfWork _unitOfWork;

        public FilmService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Film[]> GetAllAsync()
        {
            IEnumerable<FilmEntity> filmsEntitie = await _unitOfWork.FilmsRepository.GetAllAsync();

            List<Film> films = new List<Film>();
            Film film;

            foreach (FilmEntity filmEntity in filmsEntitie)
            {
                film = filmEntity.ToModel();

                foreach (CastEntity cast in filmEntity.Actors)
                {
                    ActorEntity actorEntity = await _unitOfWork.ActorsRepository.GetAsync(cast.ActorId);

                    film.Actors.Add(actorEntity.ToModel());
                }

                foreach (FilmGenreEntity filmGenreEntity in filmEntity.Genres)
                {
                    GenreEntity genreEntity = await _unitOfWork.GenresRepository.GetAsync(filmGenreEntity.GenreId);

                    film.Genres.Add(genreEntity.ToModel());
                }

                films.Add(film);
            }

            return films.ToArray();
        }

        public async Task<Film> GetAsync(Guid id)
        {
            FilmEntity filmEntity = await _unitOfWork.FilmsRepository.GetAsync(id);

            if (filmEntity == null)
            {
                return null;
            }

            Film film = filmEntity.ToModel();

            foreach (CastEntity cast in filmEntity.Actors)
            {
                ActorEntity actorEntity = await _unitOfWork.ActorsRepository.GetAsync(cast.ActorId);

                film.Actors.Add(actorEntity.ToModel());
            }

            foreach (FilmGenreEntity filmGenreEntity in filmEntity.Genres)
            {
                GenreEntity genreEntity = await _unitOfWork.GenresRepository.GetAsync(filmGenreEntity.GenreId);

                film.Genres.Add(genreEntity.ToModel());
            }

            return filmEntity.ToModel();
        }

        public async Task<Film> InsertAsync(Film film)
        {
            FilmEntity filmEntity = new FilmEntity
            {
                Id = Guid.NewGuid(),
                Name = film.Name,
                Director = film.Director,
                ReleaseDate = film.ReleaseDate,
                Country = film.Country,
                Duration = film.Duration
            };

            await _unitOfWork.FilmsRepository.InsertAsync(filmEntity);
            await _unitOfWork.SaveAsync();

            foreach (Actor actor in film.Actors)
            {
                CastEntity castsEntity = new CastEntity
                {
                    FilmId = filmEntity.Id,
                    ActorId = actor.Id
                };

                await _unitOfWork.CastsRepository.InsertAsync(castsEntity);
                await _unitOfWork.SaveAsync();

                filmEntity.Actors.Add(castsEntity);
                await _unitOfWork.FilmsRepository.UpdateAsync(filmEntity.Id);
                await _unitOfWork.SaveAsync();
            }

            foreach (Genre genre in film.Genres)
            {
                FilmGenreEntity filmGenreEntity = new FilmGenreEntity
                {
                    FilmId = filmEntity.Id,
                    GenreId = genre.Id
                };

                await _unitOfWork.FilmGenresRepository.InsertAsync(filmGenreEntity);
                await _unitOfWork.SaveAsync();

                filmEntity.Genres.Add(filmGenreEntity);
                await _unitOfWork.FilmsRepository.UpdateAsync(filmEntity.Id);
                await _unitOfWork.SaveAsync();
            }

            return filmEntity.ToModel();
        }
        public async Task RemoveAsync(Guid filmId)
        {
            if (!await _unitOfWork.FilmsRepository.ExistsAsync(filmId))
            {
                return;
            }

            await _unitOfWork.FilmsRepository.RemoveAsync(filmId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Film film)
        {
            if (!await _unitOfWork.FilmsRepository.ExistsAsync(film.Id))
            {
                return;
            }

            FilmEntity filmEntity = await _unitOfWork.FilmsRepository.GetAsync(film.Id);

            foreach (CastEntity cast in filmEntity.Actors)
            {
                await _unitOfWork.CastsRepository.RemoveAsync(filmEntity.Id, cast.ActorId);
                await _unitOfWork.SaveAsync();
            }

            foreach(Actor actor in film.Actors)
            {
                CastEntity newCastEntity = new CastEntity
                {
                    FilmId = filmEntity.Id,
                    ActorId = actor.Id
                };

                await _unitOfWork.CastsRepository.InsertAsync(newCastEntity);
                await _unitOfWork.SaveAsync();
            }

            filmEntity.Id = film.Id;
            filmEntity.Name = film.Name;
            filmEntity.Director = film.Director;
            filmEntity.ReleaseDate = film.ReleaseDate;
            filmEntity.Country = film.Country;
            filmEntity.Duration = film.Duration;

            await _unitOfWork.FilmsRepository.UpdateAsync(filmEntity.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
