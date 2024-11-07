using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class SeatRepository(CinemaContext dbContext) : RepositoryBase<Seat>(dbContext), ISeatRepository
    {
        public void CreateSeat(Guid? eventId, Guid? showtimeId, Seat seat)
        {
            seat.EventId = eventId;
            seat.ShowtimeId = showtimeId;
            Create(seat);
        }

        public void DeleteSeat(Seat seat) => Delete(seat);

        public async Task<IEnumerable<Seat>> GetAllSeatsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                  .Include(s => s.Event)
                  .Include(s => s.Showtime)
                  .Include(s => s.Showtime!.Movie)
                  .OrderBy(s => s.SeatNumber)
                  .ToListAsync();

        public async Task<Seat> GetSeatAsync(Guid id, bool trackChanges) =>
            await FindByCondition(s => s.SeatId.Equals(id), trackChanges)
                  .Include(s => s.Event)
                  .Include(s => s.Showtime)
                  .Include(s => s.Showtime!.Movie)
                  .SingleOrDefaultAsync();
    }
}