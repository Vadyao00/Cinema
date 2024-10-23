using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class EmployeeRepository(CinemaContext cinemaContext) : RepositoryBase<Employee>(cinemaContext), IEmployeeRepository
    {
        public void CreateEmployee(Employee employee) => Create(employee);

        public void DeleteEmployee(Employee employee) => Delete(employee);

        public async Task<IEnumerable<Employee>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(e => ids.Contains(e.EmployeeId), trackChanges)
                  .ToListAsync();

        public async Task<Employee> GetEmployeeAsync(Guid id, bool trackChanges) =>
            await FindByCondition(e => e.EmployeeId.Equals(id), trackChanges)
                  .SingleOrDefaultAsync();

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                  .OrderBy(e => e.Name)
                  .ToListAsync();
    }
}
