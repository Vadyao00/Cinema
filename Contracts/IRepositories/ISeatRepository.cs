using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetAllSeatsAsync(bool trackChanges);
        Task<Seat> GetSeatAsync(Guid id, bool trackChanges);
        void CreateSeat(Guid? eventId, Guid? showtimeId, Seat seat);
        void DeleteSeat(Seat seat);
    }
}
