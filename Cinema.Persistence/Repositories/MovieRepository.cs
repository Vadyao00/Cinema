using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class MovieRepository(CinemaContext dbContext) : RepositoryBase<Movie>(dbContext), IMovieRepository
    {
        public void CreateMovie(Movie movie) => Create(movie);

        public void DeleteMovie(Movie movie) => Delete(movie);

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                  .OrderBy(m => m.Title)
                  .ToListAsync();

        public async Task<IEnumerable<Movie>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(m => ids.Contains(m.MovieId), trackChanges)
                  .ToListAsync();

        public async Task<Movie> GetMovieAsync(Guid id, bool trackChanges) =>
            await FindByCondition(m => m.MovieId.Equals(id), trackChanges)
                  .SingleOrDefaultAsync();
    }
}
