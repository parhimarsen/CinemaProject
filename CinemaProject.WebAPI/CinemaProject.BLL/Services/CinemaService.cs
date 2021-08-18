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
            return _unitOfWork.CinemasRepository.GetWithInclude(cinema => cinema.Halls)
                .Select(cinema => new Cinema
                {
                    Id = cinema.Id,
                    Name = cinema.Name,
                    CityId = cinema.CityId,
                    Halls = cinema.Halls.ToModel(),
                    TypesOfSeat = cinema.TypesOfSeat.ToModel()
                });
        }

        public async Task<IQueryable<Cinema>> GetOfCity(Guid cityId)
        {
            if (!await _unitOfWork.CitiesRepository.ExistsAsync(cityId))
            {
                return null;
            }

            return _unitOfWork.CinemasRepository.GetAll()
                .Select(cinema => new Cinema
                {
                    Id = cinema.Id,
                    Name = cinema.Name,
                    CityId = cinema.CityId
                })
                .Where(cinema => cinema.CityId == cityId);
        }

        public async Task<Cinema> GetAsync(Guid id)
        {
            if (!await _unitOfWork.CinemasRepository.ExistsAsync(id))
            {
                return null;
            }

            IQueryable<CinemaEntity> cinemaQuery = _unitOfWork.CinemasRepository.GetAll();

            Cinema cinema = cinemaQuery
                .Where(cinema => cinema.Id == id)
                .Select(cinema => cinema.ToModel())
                .FirstOrDefault();

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

            cinemaEntity.Name = cinema.Name ?? cinemaEntity.Name;
            cinemaEntity.CityId = cinema.CityId ?? cinemaEntity.CityId;

            await _unitOfWork.FilmsRepository.UpdateAsync(cinemaEntity.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
