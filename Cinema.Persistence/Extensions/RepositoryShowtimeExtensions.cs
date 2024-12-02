using Cinema.Domain.Entities;
using Cinema.Persistence.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryShowtimeExtensions
    {
        public static IQueryable<Showtime> FilterShowtimes(this IQueryable<Showtime> showtimes, decimal MinTicketPrice, decimal MaxTicketPrice, TimeOnly StartTime, TimeOnly EndTime)
            => showtimes.Where(s => (s.TicketPrice >= MinTicketPrice && s.TicketPrice <= MaxTicketPrice && s.StartTime >= StartTime && s.EndTime <= EndTime));

        public static IQueryable<Showtime> SearchTitle(this IQueryable<Showtime> showtimes, string searchTitle)
        {
            if (string.IsNullOrWhiteSpace(searchTitle))
                return showtimes;

            var lowerCaseTitle = searchTitle.Trim().ToLower();

            return showtimes.Where(a => a.Movie.Title.ToLower().Contains(lowerCaseTitle));
        }

        public static IQueryable<Showtime> SearchTicketPrice(this IQueryable<Showtime> showtimes, string searchPrice)
        {
            if (string.IsNullOrWhiteSpace(searchPrice))
                return showtimes;

            var trimmedPrice = searchPrice.Trim();

            return showtimes.Where(a => a.TicketPrice.ToString().StartsWith(trimmedPrice));
        }

        public static IQueryable<Showtime> SearchMonth(this IQueryable<Showtime> showtimes, string searchMonth)
        {
            if (string.IsNullOrWhiteSpace(searchMonth))
                return showtimes;

            if (int.TryParse(searchMonth, out int month) && month >= 1 && month <= 12)
            {
                return showtimes.Where(s => s.Date.Month == month && s.Date.Year == DateTime.Now.Year);
            }

            return showtimes;
        }

        public static IQueryable<Showtime> Sort(this IQueryable<Showtime> showtimes, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return showtimes.OrderBy(e => e.Date);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Showtime>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return showtimes.OrderBy(e => e.Date);

            return showtimes.OrderBy(orderQuery);
        }
    }
}