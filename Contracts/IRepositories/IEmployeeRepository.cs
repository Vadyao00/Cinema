using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync(bool trackChanges);
        Task<Employee> GetEmployeeAsync(Guid id, bool trackChanges);
        void CreateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
    }
}
