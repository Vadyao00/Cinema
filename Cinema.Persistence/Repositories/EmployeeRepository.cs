using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Persistence.Extensions;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class EmployeeRepository(CinemaContext cinemaContext) : RepositoryBase<Employee>(cinemaContext), IEmployeeRepository
    {
        public void CreateEmployee(Employee employee) => Create(employee);

        public void DeleteEmployee(Employee employee) => Delete(employee);

        public async Task<Employee> GetEmployeeAsync(Guid id, bool trackChanges) =>
            await FindByCondition(e => e.EmployeeId.Equals(id), trackChanges)
                  .SingleOrDefaultAsync();

        public async Task<PagedList<Employee>> GetEmployeesAsync(EmployeeParameters employeeParameters, bool trackChanges)
        {
            var employees = await FindAll(trackChanges)
                  .Search(employeeParameters.searchName)
                  .Sort(employeeParameters.OrderBy)
                  .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
                  .Take(employeeParameters.PageSize)
                  .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

            return new PagedList<Employee>(employees, count, employeeParameters.PageNumber, employeeParameters.PageSize);
        }
    }
}