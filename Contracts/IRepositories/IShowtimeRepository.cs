using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;

namespace Contracts.IRepositories
{
    public interface IShowtimeRepository
    {
        Task<PagedList<Showtime>> GetAllShowtimesForMovieAsync(ShowtimeParameters showtimeParameters, Guid movieId, bool trackChanges);
        Task<PagedList<Showtime>> GetAllShowtimesAsync(ShowtimeParameters showtimeParameters, bool trackChanges);
        Task<Showtime> GetShowtimeForMovieAsync(Guid movieId, Guid id, bool trackChanges);
        Task<Showtime> GetShowtimeAsync(Guid id, bool trackChanges);
        Task<IEnumerable<Showtime>> GetAllShowtimesWithoutMetaAsync(bool trackChanges);
        void CreateShowtimeForMovie(Guid movieId, Showtime showtime);
        void DeleteShowtimeForMovie(Showtime showtime);
        Task<IEnumerable<Showtime>> GetShowtimesByIdsAsync(Guid[] ids, bool trackChanges);
    }
}