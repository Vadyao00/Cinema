using Cinema.Domain.Entities;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryEventExtensions
    {
        public static IQueryable<Event> FilterEvents(this IQueryable<Event> events, decimal MinTicketPrice, decimal MaxTicketPrice, TimeOnly StartTime, TimeOnly EndTime)
            => events.Where(e => (e.TicketPrice >= MinTicketPrice && e.TicketPrice <= MaxTicketPrice && e.StartTime >= StartTime && e.EndTime <= EndTime));

        public static IQueryable<Event> Search(this IQueryable<Event> events, string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
                return events;

            var lowerCaseName = searchName.Trim().ToLower();

            return events.Where(a => a.Name.ToLower().Contains(lowerCaseName));
        }
    }
}