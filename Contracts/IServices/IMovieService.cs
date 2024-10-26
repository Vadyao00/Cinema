using Cinema.Domain.DataTransferObjects;

namespace Contracts.IServices
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllMoviesAsync(Guid genreId, bool trackChanges);
        Task<MovieDto> GetMovieAsync(Guid genreId, Guid Id, bool trackChanges);
        Task<MovieDto> CreateMovieForGenreAsync(Guid genreId, MovieForCreationDto movie, bool trackChanges);
        Task DeleteMovieAsync(Guid genreId, Guid Id, bool trackChanges);
        Task UpdateMovieForGenreAsync(Guid genreId, Guid Id, MovieForUpdateDto movieForUpdate, bool genrTrackChanges, bool movTrackChanges);
    }
}