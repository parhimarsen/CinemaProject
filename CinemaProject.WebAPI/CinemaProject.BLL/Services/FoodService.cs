using CinemaProject.BLL.Mappers;
using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace CinemaProject.BLL.Services
{
    public class FoodService
    {
        private readonly UnitOfWork _unitOfWork;

        public FoodService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<Food> GetAll()
        {
            return _unitOfWork.FoodsRepository
                .GetAll()
                .Select(food => new Food
                {
                    Id = food.Id,
                    Name = food.Name,
                    Cost = food.Cost
                });
        }

        public async Task<Food[]> GetAllOfTicketAsync(Guid ticketId)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(ticketId))
            {
                return null;
            }

            IList<Food> foodsOfTicket = new List<Food>();

            IQueryable<TicketFoodEntity> ticketFoodQuery = _unitOfWork.TicketFoodsRepository.GetAll();

            IEnumerable<FoodEntity> foods = ticketFoodQuery
                .Where(x => x.TicketId == ticketId)
                .Select(x => x.Food);

            foreach (var food in foods)
            {
                foodsOfTicket.Add(food.ToModel());
            }

            return foodsOfTicket.ToArray();
        }

        public async Task<Food> InsertAsync(Food food)
        {
            FoodEntity foodEntity = new FoodEntity
            {
                Id = Guid.NewGuid(),
                Name = food.Name,
                Cost = food.Cost,
            };

            await _unitOfWork.FoodsRepository.InsertAsync(foodEntity);
            await _unitOfWork.SaveAsync();

            return foodEntity.ToModel();
        }

        public async Task<bool> InsertToTicketAsync(Guid ticketId, Guid foodId)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(ticketId) 
                || !await _unitOfWork.FoodsRepository.ExistsAsync(foodId))
            {
                return false;
            }

            TicketFoodEntity ticketFoodEntity = new TicketFoodEntity
            {
                TicketId = ticketId,
                FoodId = foodId
            };

            await _unitOfWork.TicketFoodsRepository.InsertAsync(ticketFoodEntity);
            await _unitOfWork.SaveAsync();

            return true;
        }

        public async Task RemoveAsync(Guid ticketId)
        {
            IQueryable<TicketFoodEntity> ticketFoodQuery = _unitOfWork.TicketFoodsRepository.GetAll();

            ticketFoodQuery
                .Where(ticket => ticket.TicketId == ticketId)
                .Delete();

            await _unitOfWork.FoodsRepository.RemoveAsync(ticketId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveFromTicketAsync(Guid ticketId, Guid foodId)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(ticketId) && !await _unitOfWork.TicketsRepository.ExistsAsync(foodId))
            {
                return;
            }

            await _unitOfWork.TicketFoodsRepository.RemoveAsync(ticketId, foodId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Food food)
        {
            if (!await _unitOfWork.FoodsRepository.ExistsAsync(food.Id))
            {
                return;
            }

            FoodEntity foodEntity = await _unitOfWork.FoodsRepository.GetAsync(food.Id);

            foodEntity.Id = food.Id;
            foodEntity.Name = food.Name;
            foodEntity.Cost = food.Cost;

            await _unitOfWork.FoodsRepository.UpdateAsync(food.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
