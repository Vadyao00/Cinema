using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class SeatRepository(CinemaContext dbContext) : RepositoryBase<Seat>(dbContext), ISeatRepository
    {
        public void CreateSeat(Seat seat) => Create(seat);

        public void DeleteSeat(Seat seat) => Delete(seat);

        public async Task<IEnumerable<Seat>> GetAllSeatsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                  .OrderBy(s => s.SeatNumber)
                  .ToListAsync();

        public async Task<IEnumerable<Seat>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(s => ids.Contains(s.SeatId), trackChanges)
                  .ToListAsync();

        public async Task<Seat> GetSeatAsync(Guid id, bool trackChanges) =>
            await FindByCondition(s => s.SeatId.Equals(id), trackChanges)
                  .SingleOrDefaultAsync();
    }
}
