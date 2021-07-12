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
    public class GenreService
    {
        private readonly UnitOfWork _unitOfWork;

        public GenreService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Genre[]> GetAllAsync()
        {
            IEnumerable<GenreEntity> genreEntities = await _unitOfWork.GenresRepository.GetAllAsync();

            return genreEntities
                .Select(genre => genre.ToModel())
                .ToArray();
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
            IEnumerable<FilmGenreEntity> filmGenreEntities = await _unitOfWork.FilmGenresRepository.GetAllAsync();

            FilmGenreEntity[] filmGenres = filmGenreEntities
                .Where(genre => genre.GenreId == id)
                .ToArray();

            foreach (var filmGenre in filmGenres)
            {
                await _unitOfWork.FilmGenresRepository.RemoveAsync(filmGenre.FilmId, filmGenre.GenreId);
            }

            await _unitOfWork.ActorsRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
