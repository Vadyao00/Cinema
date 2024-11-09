using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;

namespace Contracts.IRepositories
{
    public interface IGenreRepository
    {
        Task<PagedList<Genre>> GetAllGenresAsync(GenreParameters genreParameters, bool trackChanges);
        Task<Genre> GetGenreAsync(Guid id, bool trackChanges);
        void CreateGenre(Genre genre);
        void DeleteGenre(Genre genre);
    }
}
