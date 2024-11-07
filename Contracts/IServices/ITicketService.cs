using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;

namespace Contracts.IServices
{
    public interface ITicketService
    {
        Task<ApiBaseResponse> GetAllTicketsForSeatAsync(Guid seatId, bool trackChanges);
        Task<ApiBaseResponse> GetTicketAsync(Guid seatId, Guid Id, bool trackChanges);
        Task<ApiBaseResponse> CreateTicketForSeatAsync(Guid seatId, TicketForCreationDto ticket, bool trackChanges);
        Task<ApiBaseResponse> DeleteTicketAsync(Guid seatId, Guid Id, bool trackChanges);
        Task<ApiBaseResponse> UpdateTicketAsync(Guid seatId, Guid Id, TicketForUpdateDto ticketForUpdate, bool seatTrackChanges, bool tickTrackChanges);
    }
}