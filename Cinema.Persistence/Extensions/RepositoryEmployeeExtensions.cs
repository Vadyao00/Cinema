using Cinema.Domain.Entities;

namespace Cinema.Persistence.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchName)
        {
            if (string.IsNullOrWhiteSpace(searchName))
                return employees;

            var lowerCaseName = searchName.Trim().ToLower();

            return employees.Where(a => a.Name.ToLower().Contains(lowerCaseName));
        }
    }
}