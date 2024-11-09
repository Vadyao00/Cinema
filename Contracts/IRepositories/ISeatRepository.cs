using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;

namespace Contracts.IRepositories
{
    public interface ISeatRepository
    {
        Task<PagedList<Seat>> GetAllSeatsAsync(SeatParameters seatParameters, bool trackChanges);
        Task<Seat> GetSeatAsync(Guid id, bool trackChanges);
        void CreateSeat(Guid? eventId, Guid? showtimeId, Seat seat);
        void DeleteSeat(Seat seat);
    }
}