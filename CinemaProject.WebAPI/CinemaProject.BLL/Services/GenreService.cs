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
    public class GenreService
    {
        private readonly UnitOfWork _unitOfWork;

        public GenreService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Genre> GetAllAsync()
        {
            return _unitOfWork.GenresRepository.GetAll()
                .Select(genre => genre.ToModel());
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
    }
}
