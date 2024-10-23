using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IShowtimeRepository
    {
        Task<IEnumerable<Showtime>> GetAllShowtimesAsync(bool trackChanges);
        Task<Showtime> GetShowtimeAsync(Guid id, bool trackChanges);
        void CreateShowtime(Showtime showtime);
        Task<IEnumerable<Showtime>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteShowtime(Showtime showtime);
    }
}
