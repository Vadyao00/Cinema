using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;

namespace Contracts.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<PagedList<Employee>> GetEmployeesAsync(EmployeeParameters employeeParameters, bool trackChanges);
        Task<Employee> GetEmployeeAsync(Guid id, bool trackChanges);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(bool trackChanges);
        void CreateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);
        void Attach(Employee employee);
        Task<IEnumerable<Employee>> GetEmployeesByIdsAsync(Guid[] ids, bool trackChanges);
    }
}