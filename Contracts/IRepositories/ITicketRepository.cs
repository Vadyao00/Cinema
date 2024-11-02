using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllTicketsForSeatAsync(Guid seatId, bool trackChanges);
        Task<Ticket> GetTicketAsync(Guid id, bool trackChanges);
        void CreateTicketForSeat(Guid seatId, Ticket ticket);
        Task<IEnumerable<Ticket>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteTicket(Ticket ticket);
    }
}
