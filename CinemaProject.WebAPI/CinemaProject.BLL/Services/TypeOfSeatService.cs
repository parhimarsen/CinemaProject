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
    public class TypeOfSeatService
    {
        private readonly UnitOfWork _unitOfWork;
        private IMemoryCache _cache;
        private const string typesOfSeatCacheKey = "typesOfSeatCacheKey";
        private MemoryCacheEntryOptions _cacheOptions;

        public TypeOfSeatService(UnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _cache = memoryCache;
            _cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(2));
        }

        public List<TypeOfSeat> GetAllAsync()
        {
            return _cache.GetOrCreate(
                typesOfSeatCacheKey,
                entry =>
                {
                    entry.SetOptions(_cacheOptions);
                    return _unitOfWork.TypesOfSeatRepository.GetAll()
                        .Select(typeOfSeat => typeOfSeat.ToModel())
                        .ToList();
                }
            );
        }

        public async Task<TypeOfSeat> GetOfSeatAsync(Guid seatId)
        {
            if (!await _unitOfWork.SeatsRepository.ExistsAsync(seatId))
            {
                return null;
            }

            SeatEntity seatEntity = _unitOfWork.SeatsRepository
                .GetWithInclude(seat => seat.TypeOfSeat)
                .FirstOrDefault(seat => seat.Id == seatId);

            return seatEntity.TypeOfSeat.ToModel();
        }

        public async Task<TypeOfSeat> InsertAsync(TypeOfSeat typeOfSeat)
        {

            TypeOfSeatEntity typeOfSeatEntity = new TypeOfSeatEntity
            {
                Id = Guid.NewGuid(),
                Name = typeOfSeat.Name,
                ExtraPaymentPercent = typeOfSeat.ExtraPaymentPercent
            };

            await _unitOfWork.TypesOfSeatRepository.InsertAsync(typeOfSeatEntity);
            await _unitOfWork.SaveAsync();

            _cache.Remove(typesOfSeatCacheKey);

            return typeOfSeatEntity.ToModel();
        }

        public async Task RemoveAsync(Guid id)
        {
            if (!await _unitOfWork.TypesOfSeatRepository.ExistsAsync(id))
            {
                return;
            }

            await _unitOfWork.TypesOfSeatRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();

            _cache.Remove(typesOfSeatCacheKey);
        }

        public async Task UpdateAsync(TypeOfSeat typeOfSeat)
        {
            if (!await _unitOfWork.TypesOfSeatRepository.ExistsAsync(typeOfSeat.Id))
            {
                return;
            }

            TypeOfSeatEntity typeOfSeatEntity = await _unitOfWork.TypesOfSeatRepository.GetAsync(typeOfSeat.Id);

            typeOfSeatEntity.Name = typeOfSeat.Name;
            typeOfSeatEntity.ExtraPaymentPercent = typeOfSeat.ExtraPaymentPercent;

            await _unitOfWork.TypesOfSeatRepository.UpdateAsync(typeOfSeatEntity.Id);
            await _unitOfWork.SaveAsync();

            _cache.Remove(typesOfSeatCacheKey);
        }
    }
}
