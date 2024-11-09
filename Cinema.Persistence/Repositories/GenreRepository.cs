using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Cinema.Persistence.Repositories
{
    public class GenreRepository(CinemaContext dbContext) : RepositoryBase<Genre>(dbContext), IGenreRepository
    {
        public void CreateGenre(Genre genre) => Create(genre);

        public void DeleteGenre(Genre genre) => Delete(genre);

        public async Task<PagedList<Genre>> GetAllGenresAsync(GenreParameters genreParameters, bool trackChanges)
        {
            var genres = await FindAll(trackChanges)
                  .OrderBy(x => x.Name)
                  .Skip((genreParameters.PageNumber - 1) * genreParameters.PageSize)
                  .Take(genreParameters.PageSize)
                  .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

            return new PagedList<Genre>(genres, count, genreParameters.PageNumber, genreParameters.PageSize);
        }

        public async Task<Genre> GetGenreAsync(Guid id, bool trackChanges) =>
            await FindByCondition(g => g.GenreId.Equals(id), trackChanges)
                  .SingleOrDefaultAsync();
    }
}