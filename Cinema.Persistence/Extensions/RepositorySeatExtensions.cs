using Cinema.Domain.Entities;
using Cinema.Persistence.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Cinema.Persistence.Extensions
{
    public static class RepositorySeatExtensions
    {
        public static IQueryable<Seat> FilterSeats(this IQueryable<Seat> seats, uint MinSeatNumber, uint MaxSeatNumber)
            => seats.Where(s => (s.SeatNumber >= MinSeatNumber && s.SeatNumber <= MaxSeatNumber));

        public static IQueryable<Seat> Sort(this IQueryable<Seat> seats, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return seats.OrderBy(e => e.SeatNumber);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Seat>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return seats.OrderBy(e => e.SeatNumber);

            return seats.OrderBy(orderQuery);
        }
    }
}