using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class GenreRepository(CinemaContext dbContext) : RepositoryBase<Genre>(dbContext), IGenreRepository
    {
        public void CreateGenre(Genre genre) => Create(genre);

        public void DeleteGenre(Genre genre) => Delete(genre);

        public async Task<IEnumerable<Genre>> GetAllGenresAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                  .OrderBy(x => x.Name)
                  .ToListAsync();

        public async Task<Genre> GetGenreAsync(Guid id, bool trackChanges) =>
            await FindByCondition(g => g.GenreId.Equals(id), trackChanges)
                  .SingleOrDefaultAsync();
    }
}
