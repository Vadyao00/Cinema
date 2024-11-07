using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class TicketRepository(CinemaContext dbContext) : RepositoryBase<Ticket>(dbContext), ITicketRepository
    {
        public void CreateTicketForSeat(Guid seatId, Ticket ticket)
        {
            ticket.SeatId = seatId;
            Create(ticket);
        }

        public void DeleteTicket(Ticket ticket) => Delete(ticket);

        public async Task<IEnumerable<Ticket>> GetAllTicketsForSeatAsync(Guid seatId, bool trackChanges) =>
            await FindByCondition(t => t.SeatId.Equals(seatId), trackChanges)
                  .Include(t => t.Seat)
                  .OrderBy(t => t.TicketId)
                  .ToListAsync();

        public async Task<Ticket> GetTicketAsync(Guid id, bool trackChanges) =>
            await FindByCondition(t => t.TicketId.Equals(id), trackChanges)
                  .Include(t => t.Seat)
                  .SingleOrDefaultAsync();
    }
}