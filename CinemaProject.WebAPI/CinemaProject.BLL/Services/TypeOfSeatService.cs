using CinemaProject.BLL.Mappers;
using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.BLL.Services
{
    public class TypeOfSeatService
    {
        private readonly UnitOfWork _unitOfWork;

        public TypeOfSeatService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TypeOfSeat[] GetAllAsync()
        {
            IQueryable<TypeOfSeatEntity> typeOfSeatQuery = _unitOfWork.TypesOfSeatRepository.GetAll();

            return typeOfSeatQuery
                .Select(typeOfSeat => typeOfSeat.ToModel())
                .ToArray();
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
        }
    }
}
