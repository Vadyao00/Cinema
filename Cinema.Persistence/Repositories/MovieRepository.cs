using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Persistence.Extensions;
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

        public async Task<PagedList<Movie>> GetAllMoviesAsync(MovieParameters movieParameters, bool trackChanges)
        {
            var movies = await FindAll(trackChanges)
                  .FilterMovies(movieParameters.MinAgeRestriction, movieParameters.MaxAgeRestriction)
                  .Search(movieParameters.searchTitle)
                  .Include(m => m.Genre)
                  .Sort(movieParameters.OrderBy)
                  .Skip((movieParameters.PageNumber - 1) * movieParameters.PageSize)
                  .Take(movieParameters.PageSize)
                  .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

            return new PagedList<Movie>(movies, count, movieParameters.PageNumber, movieParameters.PageSize);
        }

        public async Task<PagedList<Movie>> GetAllMoviesForGenreAsync(MovieParameters movieParameters, Guid genreId, bool trackChanges)
        {
            var movies = await FindByCondition(m => m.GenreId.Equals(genreId), trackChanges)
                  .FilterMovies(movieParameters.MinAgeRestriction, movieParameters.MaxAgeRestriction)
                  .Search(movieParameters.searchTitle)
                  .Include(m => m.Genre)
                  .Sort(movieParameters.OrderBy)
                  .Skip((movieParameters.PageNumber - 1) * movieParameters.PageSize)
                  .Take(movieParameters.PageSize)
                  .ToListAsync();

            var count = await FindByCondition(m => m.GenreId.Equals(genreId), trackChanges).CountAsync();

            return new PagedList<Movie>(movies, count, movieParameters.PageNumber, movieParameters.PageSize);
        }

        public async Task<Movie> GetMovieAsync(Guid genreId, Guid id, bool trackChanges) =>
            await FindByCondition(m => m.MovieId.Equals(id) && m.GenreId.Equals(genreId), trackChanges)
                  .Include(m => m.Genre)
                  .SingleOrDefaultAsync();
    }
}