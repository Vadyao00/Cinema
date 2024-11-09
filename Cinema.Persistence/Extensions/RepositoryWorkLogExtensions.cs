using Cinema.Domain.Entities;

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
    }
}