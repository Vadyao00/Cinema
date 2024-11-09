using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
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

        public async Task<PagedList<Seat>> GetAllSeatsAsync(SeatParameters seatParameters, bool trackChanges)
        {
            var seats = await FindByCondition(s => s.SeatNumber >= seatParameters.MinSeatNumber && s.SeatNumber <= seatParameters.MaxSeatNumber,trackChanges)
                  .Include(s => s.Event)
                  .Include(s => s.Showtime)
                  .Include(s => s.Showtime!.Movie)
                  .OrderBy(s => s.SeatNumber)
                  .Skip((seatParameters.PageNumber - 1) * seatParameters.PageSize)
                  .Take(seatParameters.PageSize)
                  .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

            return new PagedList<Seat>(seats, count, seatParameters.PageNumber, seatParameters.PageSize);
        }

        public async Task<Seat> GetSeatAsync(Guid id, bool trackChanges) =>
            await FindByCondition(s => s.SeatId.Equals(id), trackChanges)
                  .Include(s => s.Event)
                  .Include(s => s.Showtime)
                  .Include(s => s.Showtime!.Movie)
                  .SingleOrDefaultAsync();
    }
}