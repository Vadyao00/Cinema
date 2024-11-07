using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;

namespace Contracts.IServices
{
    public interface IWorkLogService
    {
        Task<ApiBaseResponse> GetAllWorkLogsForEmployeeAsync(Guid employeeId, bool trackChanges);
        Task<ApiBaseResponse> GetWorkLogForEmployeeAsync(Guid employeeId, Guid Id, bool trackChanges);
        Task<ApiBaseResponse> CreateWorkLogForEmployeeAsync(Guid employeeId, WorkLogForCreationDto workLog, bool trackChanges);
        Task<ApiBaseResponse> DeleteWorkLogForEmployeeAsync(Guid employeeId, Guid Id, bool trackChanges);
        Task<ApiBaseResponse> UpdateWorkLogAsync(Guid employeeId, Guid Id, WorkLogForUpdateDto workLogForUpdate, bool empTrackChanges, bool wrkTrackChanges);
    }
}