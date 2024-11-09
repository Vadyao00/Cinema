using Cinema.Domain.Entities;
using Cinema.Persistence.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryActorExtensions
    {
        public static IQueryable<Actor> Search(this IQueryable<Actor> actors, string searchName)
        {
            if(string.IsNullOrWhiteSpace(searchName))
                return actors;

            var lowerCaseName = searchName.Trim().ToLower();

            return actors.Where(a => a.Name.ToLower().Contains(lowerCaseName));
        }

        public static IQueryable<Actor> Sort(this IQueryable<Actor> actors, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return actors.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Actor>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return actors.OrderBy(e => e.Name);

            return actors.OrderBy(orderQuery);
        }
    }
}