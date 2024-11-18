using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;

namespace Contracts.IRepositories
{
    public interface ITicketRepository
    {
        Task<PagedList<Ticket>> GetAllTicketsForSeatAsync(TicketParameters ticketParameters, Guid seatId, bool trackChanges);
        Task<PagedList<Ticket>> GetAllTicketsAsync(TicketParameters ticketParameters, bool trackChanges);
        Task<Ticket> GetTicketAsync(Guid id, bool trackChanges);
        void CreateTicketForSeat(Guid seatId, Ticket ticket);
        void DeleteTicket(Ticket ticket);
    }
}