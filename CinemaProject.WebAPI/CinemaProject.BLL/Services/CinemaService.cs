using CinemaProject.BLL.Mappers;
using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.BLL.Services
{
    public class CinemaService
    {
        private readonly UnitOfWork _unitOfWork;

        public CinemaService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Cinema> GetAll()
        {
            return _unitOfWork.CinemasRepository.GetAll()
                .Select(cinema => cinema.ToModel());
        }

        public IQueryable<Cinema> GetOfCity(Guid cityId)
        {
            return _unitOfWork.CinemasRepository.GetAll()
                .Select(cinema => cinema.ToModel())
                .Where(cinema => cinema.CityId == cityId);
        }

        public Cinema GetAsync(Guid id)
        {
            IQueryable<CinemaEntity> cinemaQuery = _unitOfWork.CinemasRepository.GetAll();

            Cinema cinema = cinemaQuery
                .Select(cinema => cinema.ToModel())
                .Where(cinema => cinema.Id == id)
                .FirstOrDefault();

            if(cinema == null)
            {
                return null;
            }

            return cinema;
        }

        public async Task<Cinema> InsertAsync(Cinema cinema)
        {
            CinemaEntity cinemaEntity = new CinemaEntity
            {
                Id = Guid.NewGuid(),
                Name = cinema.Name,
                CityId = cinema.CityId
            };

            await _unitOfWork.CinemasRepository.InsertAsync(cinemaEntity);
            await _unitOfWork.SaveAsync();

            return cinemaEntity.ToModel();
        }

        public async Task RemoveAsync(Guid id)
        {
            if (!await _unitOfWork.CinemasRepository.ExistsAsync(id))
            {
                return;
            }

            await _unitOfWork.CinemasRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }
        public async Task UpdateAsync(Cinema cinema)
        {
            if (!await _unitOfWork.CinemasRepository.ExistsAsync(cinema.Id))
            {
                return;
            }

            CinemaEntity cinemaEntity = await _unitOfWork.CinemasRepository.GetAsync(cinema.Id);

            cinemaEntity.Name = cinema.Name;
            cinemaEntity.CityId = cinema.CityId;

            await _unitOfWork.FilmsRepository.UpdateAsync(cinemaEntity.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
