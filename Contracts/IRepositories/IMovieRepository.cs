using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesForGenreAsync(Guid genreId, bool trackChanges);
        Task<Movie> GetMovieAsync(Guid genreId, Guid id, bool trackChanges);
        void CreateMovieForGenre(Guid genreId, Movie movie);
        void DeleteMovie(Movie movie);
    }
}