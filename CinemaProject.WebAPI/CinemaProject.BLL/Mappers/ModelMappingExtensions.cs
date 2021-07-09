using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using System.Linq;

namespace CinemaProject.BLL.Mappers
{
    public static class ModelMappingExtensions
    {
        public static User ToModel(this UserEntity @this)
        {
            return new User
            {
                Id = @this.Id,
                Email = @this.Email,
                Login = @this.Login,
                Password = @this.Password,
                IsAdmin = @this.IsAdmin,
            };
        }

        public static Ticket ToModel(this TicketEntity @this)
        {
            return new Ticket
            {
                Id = @this.Id,
                Status = @this.Status
            };
        }

        public static Food ToModel (this FoodEntity @this)
        {
            return new Food
            {
                Id = @this.Id,
                Name = @this.Name,
                Cost = @this.Cost
            };
        }
    }
}
