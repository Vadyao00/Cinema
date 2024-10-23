using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync(bool trackChanges);
        Task<Movie> GetMovieAsync(Guid id, bool trackChanges);
        void CreateMovie(Movie movie);
        Task<IEnumerable<Movie>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteMovie(Movie movie);
    }
}
