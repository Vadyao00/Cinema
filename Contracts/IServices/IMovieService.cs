using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;

namespace Contracts.IServices
{
    public interface IMovieService
    {
        Task<ApiBaseResponse> GetAllMoviesAsync(MovieParameters movieParameters, Guid genreId, bool trackChanges);
        Task<ApiBaseResponse> GetMovieAsync(Guid genreId, Guid Id, bool trackChanges);
        Task<ApiBaseResponse> CreateMovieForGenreAsync(Guid genreId, MovieForCreationDto movie, bool trackChanges);
        Task<ApiBaseResponse> DeleteMovieAsync(Guid genreId, Guid Id, bool trackChanges);
        Task<ApiBaseResponse> UpdateMovieForGenreAsync(Guid genreId, Guid Id, MovieForUpdateDto movieForUpdate, bool genrTrackChanges, bool movTrackChanges);
    }
}