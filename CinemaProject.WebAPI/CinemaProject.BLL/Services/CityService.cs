using CinemaProject.BLL.Mappers;
using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.BLL.Services
{
    public class CityService
    {
        private readonly UnitOfWork _unitOfWork;
        private IMemoryCache _cache;
        private const string citiesCacheKey = "citiesCacheKey";
        private MemoryCacheEntryOptions _cacheOptions;

        public CityService(UnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _cache = memoryCache;
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(2));
        }

        public List<City> GetAllAsync()
        {
            return _cache.GetOrCreate(
                citiesCacheKey,
                entry =>
                {
                    entry.SetOptions(_cacheOptions);
                    return _unitOfWork.CitiesRepository.GetAll()
                        .Select(city => city.ToModel())
                        .ToList();
                }
            );
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

            if (cinemaEntity == null)
            {
                return null;
            }

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

            _cache.Remove(citiesCacheKey);

            return cityEntity.ToModel();
        }

        public async Task RemoveAsync(Guid id)
        {
            if (!await _unitOfWork.CitiesRepository.ExistsAsync(id))
            {
                return;
            }

            await _unitOfWork.CitiesRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();

            CityEntity cityEntity = await _unitOfWork.CitiesRepository.GetAsync(id);
            _cache.Remove(citiesCacheKey);
        }

        public async Task UpdateAsync(City city)
        {
            if (!await _unitOfWork.CitiesRepository.ExistsAsync(city.Id))
            {
                return;
            }

            CityEntity cityEntity = await _unitOfWork.CitiesRepository.GetAsync(city.Id);

            cityEntity.Name = city.Name;

            await _unitOfWork.CitiesRepository.UpdateAsync(city.Id);
            await _unitOfWork.SaveAsync();

            _cache.Remove(citiesCacheKey);
        }
    }
}
