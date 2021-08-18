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

        public async Task<IQueryable<Seat>> GetAllOfHallAsync(Guid hallId)
        {
            if (!await _unitOfWork.HallsRepository.ExistsAsync(hallId))
            {
                return null;
            }

            return _unitOfWork.SeatsRepository
                .GetAll()
                .Where(seat => seat.HallId == hallId)
                .Select(seat => new Seat
                {
                    Id = seat.Id,
                    NumberOfSeat = seat.NumberOfSeat,
                    Row = seat.Row,
                    Column = seat.Column,
                    HallId = seat.HallId,
                    TypeOfSeatId = seat.TypeOfSeatId
                });
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

            await _unitOfWork.SeatsRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveRangeAsync(Guid hallId, Seat[] seats)
        {
            if (!await _unitOfWork.HallsRepository.ExistsAsync(hallId))
            {
                return;
            }

            foreach (Seat seat in seats)
            {
                await _unitOfWork.SeatsRepository.RemoveAsync(seat.Id);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task UpdateAsync(Seat seat)
        {
            if (!await _unitOfWork.SeatsRepository.ExistsAsync(seat.Id))
            {
                return;
            }

            SeatEntity seatEntity = await _unitOfWork.SeatsRepository.GetAsync(seat.Id);

            seatEntity.NumberOfSeat = seat.NumberOfSeat;
            seatEntity.Row = seat.Row;
            seatEntity.Column = seat.Column;
            seatEntity.HallId = seat.HallId;
            seatEntity.TypeOfSeatId = seat.TypeOfSeatId;

            await _unitOfWork.SeatsRepository.UpdateAsync(seat.Id);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateRange(Guid hallId, Seat[] seats)
        {
            if (!await _unitOfWork.HallsRepository.ExistsAsync(hallId))
            {
                return;
            }

            SeatEntity seatEntity;

            foreach (Seat seat in seats)
            {
                if (!await _unitOfWork.SeatsRepository.ExistsAsync(seat.Id))
                {
                    return;
                }

                seatEntity = await _unitOfWork.SeatsRepository.GetAsync(seat.Id);

                seatEntity.NumberOfSeat = seat.NumberOfSeat;
                seatEntity.Column = seat.Column;
                seatEntity.Row = seat.Row;
                seatEntity.HallId = seat.HallId;
                seatEntity.TypeOfSeatId = seat.TypeOfSeatId;

                await _unitOfWork.SeatsRepository.UpdateAsync(seat.Id);
                await _unitOfWork.SaveAsync();
            }
        }
    }
}
