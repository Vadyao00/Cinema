using Cinema.Domain.DataTransferObjects;

namespace Contracts.IServices
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAllTicketsAsync(bool trackChanges);
        Task<TicketDto> GetTicketAsync(Guid seatId, Guid Id, bool trackChanges);
        Task<TicketDto> CreateTicketForSeatAsync(Guid seatId, TicketForCreationDto ticket, bool trackChanges);
        Task DeleteTicketAsync(Guid Id, bool trackChanges);
        Task UpdateTicketAsync(Guid seatId, Guid Id, TicketForUpdateDto ticketForUpdate, bool seatTrackChanges, bool tickTrackChanges);
    }
}