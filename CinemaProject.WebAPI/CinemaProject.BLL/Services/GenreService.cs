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
    public class GenreService
    {
        private readonly UnitOfWork _unitOfWork;

        public GenreService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Genre> GetAll()
        {
            return _unitOfWork.GenresRepository.GetAll()
                .Select(genre => new Genre
                {
                    Id = genre.Id,
                    Name = genre.Name
                });
        }

        public async Task<Genre[]> GetAllOfFilm(Guid filmId)
        {
            if (!await _unitOfWork.FilmsRepository.ExistsAsync(filmId))
            {
                return null;
            }

            IList<Genre> genresOfFilm = new List<Genre>();

            IQueryable<FilmGenreEntity> filmGenreQuery = _unitOfWork.FilmGenresRepository.GetAll();

            IEnumerable<GenreEntity> genres = filmGenreQuery
                .Where(x => x.FilmId == filmId)
                .Select(x => x.Genre);

            foreach (var genre in genres)
            {
                genresOfFilm.Add(genre.ToModel());
            }

            return genresOfFilm.ToArray();
        }

        public async Task<Genre> InsertAsync(Genre genre)
        {
            GenreEntity genreEntity = new GenreEntity
            {
                Id = Guid.NewGuid(),
                Name = genre.Name,
            };

            await _unitOfWork.GenresRepository.InsertAsync(genreEntity);
            await _unitOfWork.SaveAsync();

            return genreEntity.ToModel();
        }

        public async Task RemoveAsync(Guid id)
        {
            IQueryable<FilmGenreEntity> filmGenreQuery = _unitOfWork.FilmGenresRepository.GetAll();

            filmGenreQuery
                .Where(genre => genre.GenreId == id)
                .Delete();

            await _unitOfWork.GenresRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Genre genre)
        {
            if (!await _unitOfWork.GenresRepository.ExistsAsync(genre.Id))
            {
                return;
            }

            GenreEntity genreEntity = await _unitOfWork.GenresRepository.GetAsync(genre.Id);

            genreEntity.Name = genre.Name;

            await _unitOfWork.GenresRepository.UpdateAsync(genreEntity.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
