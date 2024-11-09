using Cinema.Domain.Entities;
using Cinema.Persistence.Extensions.Utility;
using System.Linq.Dynamic.Core;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryWorkLogExtensions
    {
        public static IQueryable<WorkLog> Search(this IQueryable<WorkLog> workLogs, string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
                return workLogs;

            var lowerCaseName = searchName.Trim().ToLower();

            return workLogs.Where(a => a.Employee.Name.ToLower().Contains(lowerCaseName));
        }

        public static IQueryable<WorkLog> Sort(this IQueryable<WorkLog> workLogs, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return workLogs.OrderBy(e => e.StartDate);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<WorkLog>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return workLogs.OrderBy(e => e.StartDate);

            return workLogs.OrderBy(orderQuery);
        }
    }
}