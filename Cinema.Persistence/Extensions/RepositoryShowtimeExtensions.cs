using Cinema.Domain.Entities;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryShowtimeExtensions
    {
        public static IQueryable<Showtime> FilterShowtimes(this IQueryable<Showtime> showtimes, decimal MinTicketPrice, decimal MaxTicketPrice, TimeOnly StartTime, TimeOnly EndTime)
            => showtimes.Where(s => (s.TicketPrice >= MinTicketPrice && s.TicketPrice <= MaxTicketPrice && s.StartTime >= StartTime && s.EndTime <= EndTime));

        public static IQueryable<Showtime> Search(this IQueryable<Showtime> showtimes, string searchTitle)
        {
            if (string.IsNullOrWhiteSpace(searchTitle))
                return showtimes;

            var lowerCaseTitle = searchTitle.Trim().ToLower();

            return showtimes.Where(a => a.Movie.Title.ToLower().Contains(lowerCaseTitle));
        }
    }
}