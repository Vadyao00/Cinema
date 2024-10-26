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

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                  .OrderBy(t => t.TicketId)
                  .ToListAsync();

        public async Task<IEnumerable<Ticket>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(t => ids.Contains(t.TicketId),trackChanges)
                  .ToListAsync();

        public async Task<Ticket> GetTicketAsync(Guid id, bool trackChanges) =>
            await FindByCondition(t => t.TicketId.Equals(id), trackChanges)
                  .SingleOrDefaultAsync();
    }
}