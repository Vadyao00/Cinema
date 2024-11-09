using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;

namespace Contracts.IServices
{
    public interface IEmployeeService
    {
        Task<ApiBaseResponse> GetAllEmployeesAsync(EmployeeParameters employeeParameters, bool trackChanges);
        Task<ApiBaseResponse> GetEmployeeAsync(Guid employeeId, bool trackChanges);
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeForCreationDto employee);
        Task<ApiBaseResponse> DeleteEmployeeAsync(Guid employeeId, bool trackChanges);
        Task<ApiBaseResponse> UpdateEmployeeAsync(Guid employeeId, EmployeeForUpdateDto employeeForUpdate, bool trackChanges);
    }
}