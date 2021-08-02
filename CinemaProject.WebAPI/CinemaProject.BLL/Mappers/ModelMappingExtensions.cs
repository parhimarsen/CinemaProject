using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using System.Collections.Generic;
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

        public static Ticket[] ToModel(this IEnumerable<TicketEntity> ticketsEntity)
        {
            List<Ticket> tickets = new List<Ticket>();

            foreach (TicketEntity ticketEntity in ticketsEntity)
            {
                tickets.Add(ticketEntity.ToModel());
            }

            return tickets.ToArray();
        }

        public static Ticket ToModel(this TicketEntity @this)
        {
            return new Ticket
            {
                Id = @this.Id,
                Status = @this.Status,
                UserId = @this.UserId,
                SessionId = @this.SessionId,
                SeatId = @this.SeatId
            };
        }

        public static Food ToModel(this FoodEntity @this)
        {
            return new Food
            {
                Id = @this.Id,
                Name = @this.Name,
                Cost = @this.Cost
            };
        }

        public static Film ToModel(this FilmEntity @this)
        {
            return new Film
            {
                Id = @this.Id,
                Name = @this.Name,
                Director = @this.Director,
                ReleaseDate = @this.ReleaseDate,
                Country = @this.Country,
                Duration = @this.Duration
            };
        }

        public static Actor ToModel(this ActorEntity @this)
        {
            return new Actor
            {
                Id = @this.Id,
                Name = @this.Name
            };
        }

        public static Genre ToModel(this GenreEntity @this)
        {
            return new Genre
            {
                Id = @this.Id,
                Name = @this.Name
            };
        }

        public static Session ToModel(this SessionEntity @this)
        {
            return new Session
            {
                Id = @this.Id,
                ShowStart = @this.ShowStart,
                ShowEnd = @this.ShowEnd,
                Cost = @this.Cost,
                HallId = @this.HallId,
                FilmId = @this.FilmId
            };
        }
        public static Hall ToModel(this HallEntity @this)
        {
            return new Hall
            {
                Id = @this.Id,
                Name = @this.Name,
                CinemaId = @this.CinemaId,
            };
        }

        public static Seat ToModel(this SeatEntity @this)
        {
            return new Seat
            {
                Id = @this.Id,
                NumberOfSeat = @this.NumberOfSeat,
                Row = @this.Row,
                Column = @this.Column,
                HallId = @this.HallId,
                TypeOfSeatId = @this.TypeOfSeatId
            };
        }

        public static TypeOfSeat ToModel(this TypeOfSeatEntity @this)
        {
            return new TypeOfSeat
            {
                Id = @this.Id,
                Name = @this.Name,
                ExtraPaymentPercent = @this.ExtraPaymentPercent
            };
        }

        public static City ToModel(this CityEntity @this)
        {
            return new City
            {
                Id = @this.Id,
                Name = @this.Name
            };
        }

        public static Cinema ToModel(this CinemaEntity @this)
        {
            return new Cinema
            {
                Id = @this.Id,
                Name = @this.Name,
                CityId = @this.CityId
            };
        }
    }
}
