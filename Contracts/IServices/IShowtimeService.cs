using Cinema.Domain.DataTransferObjects;

namespace Contracts.IServices
{
    public interface IShowtimeService
    {
        Task<IEnumerable<ShowtimeDto>> GetAllShowtimesAsync(Guid genreId, Guid movieId, bool trackChanges);
        Task<ShowtimeDto> GetShowtimeAsync(Guid genreId, Guid movieId, Guid Id, bool trackChanges);
        Task<ShowtimeDto> CreateShowtimeForMovieAsync(Guid genreId, Guid movieId, ShowtimeForCreationDto showtime, bool trackChanges);
        Task DeleteShowtimeAsync(Guid genreId, Guid movieId, Guid Id, bool trackChanges);
        Task UpdateShowtimeAsync(Guid genreId, Guid movieId, Guid Id, ShowtimeForUpdateDto showtimeForUpdate, bool movTrackChanges, bool shwTrackChanges);
    }
}