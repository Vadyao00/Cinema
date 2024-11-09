using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Persistence.Extensions;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class ShowtimeRepository(CinemaContext dbContext) : RepositoryBase<Showtime>(dbContext), IShowtimeRepository
    {
        public void CreateShowtimeForMovie(Guid movieId, Showtime showtime)
        {
            showtime.MovieId = movieId;
            Create(showtime);
        }

        public void DeleteShowtimeForMovie(Showtime showtime) => Delete(showtime);

        public async Task<PagedList<Showtime>> GetAllShowtimesForMovieAsync(ShowtimeParameters showtimeParameters, Guid movieId, bool trackChanges)
        {
            var showtimes = await FindByCondition(s => s.MovieId.Equals(movieId), trackChanges)
                  .FilterShowtimes(showtimeParameters.MinTicketPrice, showtimeParameters.MaxTicketPrice, showtimeParameters.StartTime, showtimeParameters.EndTime)
                  .Search(showtimeParameters.searchTitle)
                  .Include(s => s.Movie)
                  .OrderBy(s => s.StartTime)
                  .Skip((showtimeParameters.PageNumber - 1) * showtimeParameters.PageSize)
                  .Take(showtimeParameters.PageSize)
                  .ToListAsync();

            var count = await FindByCondition(s => s.MovieId.Equals(movieId), trackChanges).CountAsync();

            return new PagedList<Showtime>(showtimes, count, showtimeParameters.PageNumber, showtimeParameters.PageSize);
        }

        public async Task<Showtime> GetShowtimeForMovieAsync(Guid movieId, Guid id, bool trackChanges) =>
            await FindByCondition(s => s.ShowtimeId.Equals(id) && s.MovieId.Equals(movieId), trackChanges)
                  .Include(s => s.Movie)
                  .SingleOrDefaultAsync();

        public async Task<Showtime> GetShowtimeAsync(Guid id, bool trackChanges) =>
            await FindByCondition(s => s.ShowtimeId.Equals(id), trackChanges)
                  .Include(s => s.Movie)
                  .SingleOrDefaultAsync();
    }
}