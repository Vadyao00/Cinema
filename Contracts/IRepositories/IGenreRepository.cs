using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetAllGenresAsync(bool trackChanges);
        Task<Genre> GetGenreAsync(Guid id, bool trackChanges);
        void CreateGenre(Genre genre);
        void DeleteGenre(Genre genre);
    }
}
