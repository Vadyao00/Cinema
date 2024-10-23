using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync(bool trackChanges);
        Task<Employee> GetEmployeeAsync(Guid id, bool trackChanges);
        Task<IEnumerable<Employee>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void CreateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
