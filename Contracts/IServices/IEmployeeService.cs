using Cinema.Domain.DataTransferObjects;

namespace Contracts.IServices
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(bool trackChanges);
        Task<EmployeeDto> GetEmployeeAsync(Guid employeeId, bool trackChanges);
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeForCreationDto employee);
        Task<IEnumerable<EmployeeDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        Task DeleteEmployeeAsync(Guid employeeId, bool trackChanges);
        Task UpdateEmployeeAsync(Guid employeeId, EmployeeForUpdateDto employeeForUpdate, bool trackChanges);
    }
}
