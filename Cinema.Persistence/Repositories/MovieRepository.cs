using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class MovieRepository(CinemaContext dbContext) : RepositoryBase<Movie>(dbContext), IMovieRepository
    {
        public void CreateMovieForGenre(Guid genreId, Movie movie)
        {
            movie.GenreId = genreId;
            Create(movie);
        }

        public void DeleteMovie(Movie movie) => Delete(movie);

        public async Task<IEnumerable<Movie>> GetAllMoviesForGenreAsync(Guid genreId, bool trackChanges) =>
            await FindByCondition(m => m.GenreId.Equals(genreId), trackChanges)
                  .OrderBy(m => m.Title)
                  .ToListAsync();

        public async Task<IEnumerable<Movie>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(m => ids.Contains(m.MovieId), trackChanges)
                  .ToListAsync();

        public async Task<Movie> GetMovieAsync(Guid genreId, Guid id, bool trackChanges) =>
            await FindByCondition(m => m.MovieId.Equals(id) && m.GenreId.Equals(genreId), trackChanges)
                  .SingleOrDefaultAsync();
    }
}