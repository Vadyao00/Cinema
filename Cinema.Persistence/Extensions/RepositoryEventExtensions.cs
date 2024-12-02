using Cinema.Domain.Entities;
using Cinema.Persistence.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryEventExtensions
    {
        public static IQueryable<Event> FilterEvents(this IQueryable<Event> events, decimal MinTicketPrice, decimal MaxTicketPrice, TimeOnly StartTime, TimeOnly EndTime, DateOnly StartDate, DateOnly EndDate)
            => events.Where(e => (e.TicketPrice >= MinTicketPrice && e.TicketPrice <= MaxTicketPrice && e.StartTime >= StartTime && e.EndTime <= EndTime && e.Date >= StartDate && e.Date <= EndDate));

        public static IQueryable<Event> Search(this IQueryable<Event> events, string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
                return events;

            var lowerCaseName = searchName.Trim().ToLower();

            return events.Where(a => a.Name.ToLower().Contains(lowerCaseName));
        }
        
        public static IQueryable<Event> Sort(this IQueryable<Event> events, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return events.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Event>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return events.OrderBy(e => e.Name);

            return events.OrderBy(orderQuery);
        }
    }
}