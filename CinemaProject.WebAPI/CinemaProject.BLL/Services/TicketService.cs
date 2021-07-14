using CinemaProject.BLL.Mappers;
using CinemaProject.BLL.Models;
using CinemaProject.DAL.Entities;
using CinemaProject.DAL.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaProject.BLL.Services
{
    public class TicketService
    {
        private readonly UnitOfWork _unitOfWork;

        public TicketService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Ticket[]> GetAllOfUserAsync(Guid userId)
        {
            if (!await _unitOfWork.UsersRepository.ExistsAsync(userId))
            {
                return null;
            }

            UserEntity userEntity = _unitOfWork.UsersRepository
                .GetWithInclude(user => user.Tickets)
                .FirstOrDefault(user => user.Id == userId);

            return userEntity.Tickets
                .Select(ticket => ticket.ToModel())
                .ToArray();
        }

        public async Task<Ticket> GetAsync(Guid id)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(id))
            {
                return null;
            }

            TicketEntity ticketEntity = await _unitOfWork.TicketsRepository.GetAsync(id);

            return ticketEntity.ToModel();
        }

        public async Task<Ticket> InsertAsync(Ticket ticket)
        {
            if (!await _unitOfWork.UsersRepository.ExistsAsync(ticket.UserId) && !await _unitOfWork.SessionsRepository.ExistsAsync(ticket.SessionId))
            {
                return null;
            }

            UserEntity userEntity = await _unitOfWork.UsersRepository.GetAsync(ticket.UserId);
            SessionEntity sessionEntity = await _unitOfWork.SessionsRepository.GetAsync(ticket.SessionId);

            TicketEntity newTicket = new TicketEntity
            {
                Id = Guid.NewGuid(),
                Status = false,
                UserId = ticket.UserId,
                SessionId = ticket.SessionId,
                SeatId = ticket.SeatId,
            };

            await _unitOfWork.TicketsRepository.InsertAsync(newTicket);
            await _unitOfWork.SaveAsync();

            userEntity.Tickets.Add(newTicket);
            await _unitOfWork.UsersRepository.UpdateAsync(userEntity.Id);
            await _unitOfWork.SaveAsync();

            sessionEntity.Tickets.Add(newTicket);
            await _unitOfWork.SessionsRepository.UpdateAsync(userEntity.Id);
            await _unitOfWork.SaveAsync();

            return newTicket.ToModel();
        }

        public async Task ProofOfPayment(Guid id)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(id))
            {
                return;
            }

            TicketEntity ticketEntity = await _unitOfWork.TicketsRepository.GetAsync(id);

            ticketEntity.Status = true;

            await _unitOfWork.TicketsRepository.UpdateAsync(id);
            await _unitOfWork.SaveAsync();

            SeatEntity seatEntity = await _unitOfWork.SeatsRepository.GetAsync(ticketEntity.SeatId);
            TypeOfSeatEntity typeOfSeatEntity = await _unitOfWork.TypesOfSeatRepository.GetAsync(seatEntity.TypeOfSeatId);
            SessionEntity sessionEntity = await _unitOfWork.SessionsRepository.GetAsync(ticketEntity.SessionId);

            TicketSeatEntity reservation = new TicketSeatEntity
            {
                SeatId = ticketEntity.SeatId,
                TicketId = id,
                CostWithPercent = sessionEntity.Cost * typeOfSeatEntity.ExtraPaymentPercent
            };

            await _unitOfWork.TicketSeatsRepository.InsertAsync(reservation);
            await _unitOfWork.SaveAsync();

            seatEntity.SeatReservations.Add(reservation);
            await _unitOfWork.SeatsRepository.UpdateAsync(seatEntity.Id);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(id))
            {
                return;
            }

            TicketEntity ticketEntity = _unitOfWork.TicketsRepository
                .GetWithInclude(ticket => ticket.Food)
                .FirstOrDefault(ticket => ticket.Id == id);

            UserEntity userEntity = await _unitOfWork.UsersRepository.GetAsync(ticketEntity.UserId);
            userEntity.Tickets.Remove(ticketEntity);
            await _unitOfWork.SaveAsync();

            SessionEntity sesionEntity = await _unitOfWork.SessionsRepository.GetAsync(ticketEntity.SessionId);
            sesionEntity.Tickets.Remove(ticketEntity);
            await _unitOfWork.SaveAsync();

            await _unitOfWork.TicketSeatsRepository.RemoveAsync(id, ticketEntity.SeatId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(ticket.Id))
            {
                return;
            }

            TicketEntity ticketEntity = await _unitOfWork.TicketsRepository.GetAsync(ticket.Id);

            ticketEntity.SessionId = ticket.SessionId;

            await _unitOfWork.TicketsRepository.UpdateAsync(ticket.Id);
            await _unitOfWork.SaveAsync();
        }
    }
}
