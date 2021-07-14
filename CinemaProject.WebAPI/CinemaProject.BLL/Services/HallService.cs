using CinemaProject.BLL.Mappers;
using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.BLL.Services
{
    public class HallService
    {
        private readonly UnitOfWork _unitOfWork;

        public HallService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Hall[]> GetAllOfCinemaAsync(Guid cinemaId)
        {
            if (!await _unitOfWork.CinemasRepository.ExistsAsync(cinemaId))
            {
                return null;
            }

            CinemaEntity cinemaEntity = _unitOfWork.CinemasRepository
                .GetWithInclude(cinema => cinema.Halls)
                .FirstOrDefault(cinema => cinema.Id == cinemaId);

            return cinemaEntity.Halls
                .Select(hall => hall.ToModel())
                .ToArray();
        }

        public async Task<Hall> GetAsync(Guid id)
        {
            if (!await _unitOfWork.HallsRepository.ExistsAsync(id))
            {
                return null;
            }

            HallEntity hallEntity = await _unitOfWork.HallsRepository.GetAsync(id);

            return hallEntity.ToModel();
        }

        public async Task<Hall> InsertAsync(Hall hall)
        {
            if (!await _unitOfWork.CinemasRepository.ExistsAsync(hall.CinemaId))
            {
                return null;
            }

            CinemaEntity cinemaEntity = await _unitOfWork.CinemasRepository.GetAsync(hall.CinemaId);

            HallEntity hallEntity = new HallEntity
            {
                Id = Guid.NewGuid(),
                Name = hall.Name,
                CinemaId = hall.CinemaId
            };

            await _unitOfWork.HallsRepository.InsertAsync(hallEntity);
            await _unitOfWork.SaveAsync();

            cinemaEntity.Halls.Add(hallEntity);
            await _unitOfWork.CinemasRepository.UpdateAsync(hallEntity.CinemaId);
            await _unitOfWork.SaveAsync();

            return hallEntity.ToModel();
        }

        public async Task UpdateAsync(Hall hall)
        {
            if (!await _unitOfWork.HallsRepository.ExistsAsync(hall.Id))
            {
                return;
            }

            HallEntity hallEntity = await _unitOfWork.HallsRepository.GetAsync(hall.Id);

            hallEntity.Name = hall.Name;
            hallEntity.CinemaId = hall.CinemaId;

            await _unitOfWork.HallsRepository.InsertAsync(hallEntity);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            if (!await _unitOfWork.HallsRepository.ExistsAsync(id))
            {
                return;
            }

            HallEntity hallEntity = _unitOfWork.HallsRepository
                .GetWithInclude(hall => hall.Seats)
                .FirstOrDefault(hall => hall.Id == id);

            CinemaEntity cinemaEntity = await _unitOfWork.CinemasRepository.GetAsync(hallEntity.CinemaId);
            cinemaEntity.Halls.Remove(hallEntity);
            await _unitOfWork.SaveAsync();
        }
    }
}
