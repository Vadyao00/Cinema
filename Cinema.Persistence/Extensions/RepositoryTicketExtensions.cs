using Cinema.Domain.Entities;
using Cinema.Persistence.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryTicketExtensions
    {
        public static IQueryable<Ticket> FilterTickets(this IQueryable<Ticket> tickets, uint MinSeatNumber, uint MaxSeatNumber)
            => tickets.Where(t => (t.Seat.SeatNumber >= MinSeatNumber && t.Seat.SeatNumber <= MaxSeatNumber));

        public static IQueryable<Ticket> Sort(this IQueryable<Ticket> tickets, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return tickets.OrderBy(e => e.PurchaseDate);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Ticket>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return tickets.OrderBy(e => e.PurchaseDate);

            return tickets.OrderBy(orderQuery);
        }
    }
}