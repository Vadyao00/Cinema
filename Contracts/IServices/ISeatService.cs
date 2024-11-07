using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;

namespace Contracts.IServices
{
    public interface ISeatService
    {
        Task<ApiBaseResponse> GetAllSeatsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetSeatAsync(Guid Id, bool trackChanges);
        Task<ApiBaseResponse> CreateSeatForShowtimeOrEventAsync(Guid? showtimeId, Guid? eventId, SeatForCreationDto seat, bool trackChanges);
        Task<ApiBaseResponse> DeleteSeatAsync(Guid Id, bool trackChanges);
        Task<ApiBaseResponse> UpdateSeatAsync(Guid Id, SeatForUpdateDto seatForUpdate, bool seatTrackChanges);
    }
}