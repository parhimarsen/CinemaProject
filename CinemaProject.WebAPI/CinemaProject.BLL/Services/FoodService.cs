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
    public class FoodService
    {
        private readonly UnitOfWork _unitOfWork;

        public FoodService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Food[]> GetAllOfTicketAsync(Guid ticketId)
        {
            if (!await _unitOfWork.UsersRepository.ExistsAsync(ticketId))
            {
                return null;
            }

            IList<Food> foodsOfTicket = new List<Food>();

            IEnumerable<TicketFoodEntity> foodsEntity = await _unitOfWork.TicketFoodRepository.GetAllAsync();

            TicketFoodEntity[] foods = foodsEntity
                .Where(food => food.TicketId == ticketId)
                .ToArray();

            foreach (var food in foods)
            {
                FoodEntity foodEntity = await _unitOfWork.FoodRepository.GetAsync(food.TicketId);

                foodsOfTicket.Add(foodEntity.ToModel());
            }

            return foodsOfTicket.ToArray();
        }

        public async Task<Food> InsertAsync(Guid ticketId, Food food)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(ticketId))
            {
                return null;
            }

            FoodEntity foodEntity = new FoodEntity
            {
                Id = Guid.NewGuid(),
                Name = food.Name,
                Cost = food.Cost,
                TicketId = ticketId
            };

            TicketFoodEntity ticketFoodEntity = new TicketFoodEntity
            {
                TicketId = ticketId,
                FoodId = foodEntity.Id
            };

            await _unitOfWork.FoodRepository.InsertAsync(foodEntity);
            await _unitOfWork.SaveAsync();

            await _unitOfWork.TicketFoodRepository.InsertAsync(ticketFoodEntity);
            await _unitOfWork.SaveAsync();

            return foodEntity.ToModel();
        }

        public async Task RemoveAsync(Guid ticketId)
        {
            IEnumerable<TicketFoodEntity> ticketFoodsEntity = await _unitOfWork.TicketFoodRepository.GetAllAsync();

            TicketFoodEntity[] ticketFoodsEntityOfTicket = ticketFoodsEntity
                .Where(ticket => ticket.TicketId == ticketId)
                .ToArray();

            foreach (var ticketFoodEntityOfTicket in ticketFoodsEntityOfTicket)
            {
                await _unitOfWork.TicketFoodRepository.RemoveAsync(ticketFoodEntityOfTicket.TicketId, ticketFoodEntityOfTicket.FoodId);
            }

            await _unitOfWork.FoodRepository.RemoveAsync(ticketId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Food food)
        {
            if (!await _unitOfWork.FoodRepository.ExistsAsync(food.Id))
            {
                return;
            }

            FoodEntity foodEntity = await _unitOfWork.FoodRepository.GetAsync(food.Id);

            foodEntity.Id = food.Id;
            foodEntity.Name = food.Name;
            foodEntity.Cost = food.Cost;

            await _unitOfWork.FoodRepository.UpdateAsync(food.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
