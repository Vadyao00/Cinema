using Cinema.Domain.Entities;
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

        public async Task<IEnumerable<Showtime>> GetAllShowtimesForMovieAsync(Guid movieId, bool trackChanges) =>
            await FindByCondition(s => s.MovieId.Equals(movieId),trackChanges)
                  .Include(s => s.Movie)
                  .OrderBy(s => s.StartTime)
                  .ToListAsync();

        public async Task<IEnumerable<Showtime>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(s => ids.Contains(s.ShowtimeId), trackChanges)
                  .ToListAsync();

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