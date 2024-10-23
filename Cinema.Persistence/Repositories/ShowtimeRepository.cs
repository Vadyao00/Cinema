using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class ShowtimeRepository(CinemaContext dbContext) : RepositoryBase<Showtime>(dbContext), IShowtimeRepository
    {
        public void CreateShowtime(Showtime showtime) => Create(showtime);

        public void DeleteShowtime(Showtime showtime) => Delete(showtime);

        public async Task<IEnumerable<Showtime>> GetAllShowtimesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                  .OrderBy(s => s.StartTime)
                  .ToListAsync();

        public async Task<IEnumerable<Showtime>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(s => ids.Contains(s.ShowtimeId), trackChanges)
                  .ToListAsync();

        public async Task<Showtime> GetShowtimeAsync(Guid id, bool trackChanges) =>
            await FindByCondition(s => s.ShowtimeId.Equals(id), trackChanges)
                  .SingleOrDefaultAsync();
    }
}
