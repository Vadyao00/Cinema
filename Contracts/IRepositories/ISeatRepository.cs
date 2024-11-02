using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetAllSeatsAsync(bool trackChanges);
        Task<Seat> GetSeatAsync(Guid id, bool trackChanges);
        void CreateSeat(Guid? eventId, Guid? showtimeId, Seat seat);
        Task<IEnumerable<Seat>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteSeat(Seat seat);
    }
}
