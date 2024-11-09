using Cinema.Domain.Entities;
using Cinema.Persistence.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryGenreExtensions
    {
        public static IQueryable<Genre> Search(this IQueryable<Genre> genres, string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
                return genres;

            var lowerCaseName = searchName.Trim().ToLower();

            return genres.Where(a => a.Name.ToLower().Contains(lowerCaseName));
        }

        public static IQueryable<Genre> Sort(this IQueryable<Genre> genres, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return genres.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Genre>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return genres.OrderBy(e => e.Name);

            return genres.OrderBy(orderQuery);
        }
    }
}