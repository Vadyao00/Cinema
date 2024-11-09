using Cinema.Domain.Entities;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryTicketExtensions
    {
        public static IQueryable<Ticket> FilterTickets(this IQueryable<Ticket> tickets, uint MinSeatNumber, uint MaxSeatNumber)
            => tickets.Where(t => (t.Seat.SeatNumber >= MinSeatNumber && t.Seat.SeatNumber <= MaxSeatNumber));
    }
}