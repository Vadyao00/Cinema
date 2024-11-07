using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;

namespace Contracts.IServices
{
    public interface IShowtimeService
    {
        Task<ApiBaseResponse> GetAllShowtimesAsync(Guid genreId, Guid movieId, bool trackChanges);
        Task<ApiBaseResponse> GetShowtimeAsync(Guid genreId, Guid movieId, Guid Id, bool trackChanges);
        Task<ApiBaseResponse> CreateShowtimeForMovieAsync(Guid genreId, Guid movieId, ShowtimeForCreationDto showtime, bool trackChanges);
        Task<ApiBaseResponse> DeleteShowtimeAsync(Guid genreId, Guid movieId, Guid Id, bool trackChanges);
        Task<ApiBaseResponse> UpdateShowtimeAsync(Guid genreId, Guid movieId, Guid Id, ShowtimeForUpdateDto showtimeForUpdate, bool movTrackChanges, bool shwTrackChanges);
    }
}