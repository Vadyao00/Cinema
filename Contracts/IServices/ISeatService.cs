using Cinema.Domain.DataTransferObjects;

namespace Contracts.IServices
{
    public interface ISeatService
    {
        Task<IEnumerable<SeatDto>> GetAllSeatsAsync(bool trackChanges);
        Task<SeatDto> GetSeatAsync(Guid Id, bool trackChanges);
        Task<SeatDto> CreateSeatForShowtimeOrEventAsync(Guid? showtimeId, Guid? eventId, SeatForCreationDto seat, bool trackChanges);
        Task DeleteSeatAsync(Guid Id, bool trackChanges);
        Task UpdateSeatAsync(Guid Id, SeatForUpdateDto seatForUpdate, bool movTrackChanges);
    }
}