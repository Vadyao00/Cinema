using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;

namespace Contracts.IRepositories
{
    public interface IMovieRepository
    {
        Task<PagedList<Movie>> GetAllMoviesForGenreAsync(MovieParameters movieParameters, Guid genreId, bool trackChanges);
        Task<PagedList<Movie>> GetAllMoviesAsync(MovieParameters movieParameters, bool trackChanges);
        Task<Movie> GetMovieAsync(Guid id, bool trackChanges);
        void CreateMovieForGenre(Guid genreId, Movie movie);
        void DeleteMovie(Movie movie);
    }
}