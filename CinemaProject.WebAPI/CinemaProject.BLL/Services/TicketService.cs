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

        public async Task<IQueryable<Ticket>> GetAllOfUserAsync(Guid userId)
        {
            if (!await _unitOfWork.UsersRepository.ExistsAsync(userId))
            {
                return null;
            }

            return _unitOfWork.TicketsRepository
                .GetAll()
                .Where(ticket => ticket.UserId == userId)
                .Select(ticket => new Ticket
                {
                    Id = ticket.Id,
                    Status = ticket.Status,
                    UserId = ticket.UserId,
                    SessionId = ticket.SessionId,
                    SeatId = ticket.SeatId
                });
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
            if (!await _unitOfWork.UsersRepository.ExistsAsync(ticket.UserId)
                || !await _unitOfWork.SessionsRepository.ExistsAsync(ticket.SessionId)
                || !await _unitOfWork.SeatsRepository.ExistsAsync(ticket.SeatId))
            {
                return null;
            }

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

            return newTicket.ToModel();
        }

        public async Task<ConfirmPaymentResponse> ConfirmPayment(Guid id)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(id))
            {

                return new ConfirmPaymentResponse
                {
                    Response = "Ticket is not exist",
                    IsConfirm = false
                };
            }

            TicketEntity ticketEntity = await _unitOfWork.TicketsRepository.GetAsync(id);

            if (ticketEntity.Status)
            {
                return new ConfirmPaymentResponse
                {
                    Response = "Allredy payed",
                    IsConfirm = false
                };
            }

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
                CostWithPercent = sessionEntity.Cost * (1M + Convert.ToDecimal(typeOfSeatEntity.ExtraPaymentPercent) / 100M)
            };

            await _unitOfWork.TicketSeatsRepository.InsertAsync(reservation);
            await _unitOfWork.SaveAsync();

            return new ConfirmPaymentResponse
            {
                Response = "Payment Accepted",
                IsConfirm = true
            };
        }

        public async Task RemoveAsync(Guid id)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(id))
            {
                return;
            }

            await _unitOfWork.TicketsRepository.RemoveAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            if (!await _unitOfWork.TicketsRepository.ExistsAsync(ticket.Id)
                || !await _unitOfWork.SessionsRepository.ExistsAsync(ticket.SessionId))
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
