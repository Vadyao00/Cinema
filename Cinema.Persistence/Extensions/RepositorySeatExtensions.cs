using Cinema.Domain.Entities;

namespace Cinema.Persistence.Extensions
{
    public static class RepositorySeatExtensions
    {
        public static IQueryable<Seat> FilterSeats(this IQueryable<Seat> seats, uint MinSeatNumber, uint MaxSeatNumber)
            => seats.Where(s => (s.SeatNumber >= MinSeatNumber && s.SeatNumber <= MaxSeatNumber));
    }
}