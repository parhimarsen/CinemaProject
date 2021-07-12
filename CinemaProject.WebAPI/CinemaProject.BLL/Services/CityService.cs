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
    public class CityService
    {
        private readonly UnitOfWork _unitOfWork;

        public CityService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<City[]> GetAllAsync()
        {
            IEnumerable<CityEntity> citiesEntity = await _unitOfWork.CitiesRepository.GetAllAsync();

            return citiesEntity
                .Select(city => city.ToModel())
                .ToArray();
        }

        public async Task<City> GetOfCinemaAsync(Guid cinemaId)
        {
            if (!await _unitOfWork.CinemasRepository.ExistsAsync(cinemaId))
            {
                return null;
            }

            CinemaEntity cinemaEntity = _unitOfWork.CinemasRepository
                .GetWithInclude(cinema => cinema.City)
                .FirstOrDefault(cinema => cinema.Id == cinemaId);

            return cinemaEntity.City.ToModel();
        }

        public async Task<City> InsertAsync(City city)
        {

            CityEntity cityEntity = new CityEntity
            {
                Id = Guid.NewGuid(),
                Name = city.Name
            };

            await _unitOfWork.CitiesRepository.InsertAsync(cityEntity);
            await _unitOfWork.SaveAsync();

            return cityEntity.ToModel();
        }

        public async Task RemoveAsync(Guid id)
        {
            await _unitOfWork.CitiesRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
