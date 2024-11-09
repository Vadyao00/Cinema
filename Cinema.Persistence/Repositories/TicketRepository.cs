using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
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

        public async Task<PagedList<Ticket>> GetAllTicketsForSeatAsync(TicketParameters ticketParameters, Guid seatId, bool trackChanges)
        {
            var tickets = await FindByCondition(t => t.SeatId.Equals(seatId)
                                                && t.Seat.SeatNumber >= ticketParameters.MinSeatNumber && t.Seat.SeatNumber <= ticketParameters.MaxSeatNumber, trackChanges)
                  .Include(t => t.Seat)
                  .OrderBy(t => t.TicketId)
                  .Skip((ticketParameters.PageNumber - 1) * ticketParameters.PageSize)
                  .Take(ticketParameters.PageSize)
                  .ToListAsync();

            var count = await FindByCondition(t => t.SeatId.Equals(seatId), trackChanges).CountAsync();

            return new PagedList<Ticket>(tickets, count, ticketParameters.PageNumber, ticketParameters.PageSize);
        }

        public async Task<Ticket> GetTicketAsync(Guid id, bool trackChanges) =>
            await FindByCondition(t => t.TicketId.Equals(id), trackChanges)
                  .Include(t => t.Seat)
                  .SingleOrDefaultAsync();
    }
}