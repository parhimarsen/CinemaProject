using CinemaProject.BLL.Mappers;
using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.BLL.Services
{
    public class SeatService
    {
        private readonly UnitOfWork _unitOfWork;

        public SeatService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Seat[]> GetAllOfHallAsync(Guid hallId)
        {
            if (!await _unitOfWork.HallsRepository.ExistsAsync(hallId))
            {
                return null;
            }

            HallEntity hallEntity = _unitOfWork.HallsRepository
                .GetWithInclude(hall => hall.Seats)
                .FirstOrDefault(hall => hall.Id == hallId);

            return hallEntity.Seats
                .Select(seat => seat.ToModel())
                .ToArray();
        }

        public async Task<Seat> GetAsync(Guid seatId)
        {
            if (!await _unitOfWork.SeatsRepository.ExistsAsync(seatId))
            {
                return null;
            }

            SeatEntity seatEntity = await _unitOfWork.SeatsRepository.GetAsync(seatId);

            return seatEntity.ToModel();
        }

        public async Task<Seat> InsertAsync(Seat seat)
        {
            if (!await _unitOfWork.HallsRepository.ExistsAsync(seat.HallId))
            {
                return null;
            }

            HallEntity hallEntity = await _unitOfWork.HallsRepository.GetAsync(seat.HallId);

            SeatEntity seatEntity = new SeatEntity
            {
                Id = Guid.NewGuid(),
                NumberOfSeat = seat.NumberOfSeat,
                Row = seat.Row,
                Column = seat.Column,
                HallId = seat.HallId,
                TypeOfSeatId = seat.TypeOfSeatId
            };

            await _unitOfWork.SeatsRepository.InsertAsync(seatEntity);
            await _unitOfWork.SaveAsync();

            hallEntity.Seats.Add(seatEntity);
            await _unitOfWork.HallsRepository.UpdateAsync(hallEntity.Id);
            await _unitOfWork.SaveAsync();

            return seatEntity.ToModel();
        }

        public async Task<Seat[]> InsertRangeAsync(Guid hallId, Seat[] seats)
        {
            if (!await _unitOfWork.HallsRepository.ExistsAsync(hallId))
            {
                return null;
            }

            SeatEntity seatEntity;

            for (int i = 0; i < seats.Length; i++)
            {
                seatEntity = new SeatEntity
                {
                    Id = Guid.NewGuid(),
                    NumberOfSeat = seats[i].NumberOfSeat,
                    Row = seats[i].Row,
                    Column = seats[i].Column,
                    HallId = seats[i].HallId,
                    TypeOfSeatId = seats[i].TypeOfSeatId
                };

                await _unitOfWork.SeatsRepository.InsertAsync(seatEntity);
                await _unitOfWork.SaveAsync();
            }

            return seats;
        }

        public async Task RemoveAsync(Guid id)
        {
            if (!await _unitOfWork.SeatsRepository.ExistsAsync(id))
            {
                return;
            }

            SeatEntity seatEntity = await _unitOfWork.SeatsRepository.GetAsync(id);

            HallEntity hallEntity = await _unitOfWork.HallsRepository.GetAsync(seatEntity.HallId);
            hallEntity.Seats.Remove(seatEntity);
            await _unitOfWork.SaveAsync();
        }
    }
}
