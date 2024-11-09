using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;

namespace Contracts.IRepositories
{
    public interface IShowtimeRepository
    {
        Task<PagedList<Showtime>> GetAllShowtimesForMovieAsync(ShowtimeParameters showtimeParameters, Guid movieId, bool trackChanges);
        Task<Showtime> GetShowtimeForMovieAsync(Guid movieId, Guid id, bool trackChanges);
        Task<Showtime> GetShowtimeAsync(Guid id, bool trackChanges);
        void CreateShowtimeForMovie(Guid movieId, Showtime showtime);
        void DeleteShowtimeForMovie(Showtime showtime);
    }
}