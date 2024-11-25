using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Persistence.Extensions;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class GenreRepository(CinemaContext dbContext) : RepositoryBase<Genre>(dbContext), IGenreRepository
    {
        public void CreateGenre(Genre genre) => Create(genre);

        public void DeleteGenre(Genre genre) => Delete(genre);

        public async Task<PagedList<Genre>> GetAllGenresAsync(GenreParameters genreParameters, bool trackChanges)
        {
            var genres = await FindAll(trackChanges)
                  .Search(genreParameters.searchName)
                  .Sort(genreParameters.OrderBy)
                  .Skip((genreParameters.PageNumber - 1) * genreParameters.PageSize)
                  .Take(genreParameters.PageSize)
                  .ToListAsync();

            var count = await FindAll(trackChanges).Search(genreParameters.searchName).CountAsync();

            return new PagedList<Genre>(genres, count, genreParameters.PageNumber, genreParameters.PageSize);
        }

        public async Task<Genre> GetGenreAsync(Guid id, bool trackChanges) =>
            await FindByCondition(g => g.GenreId.Equals(id), trackChanges)
                  .SingleOrDefaultAsync();
    }
}