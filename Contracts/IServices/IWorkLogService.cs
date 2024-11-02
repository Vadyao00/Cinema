using Cinema.Domain.DataTransferObjects;

namespace Contracts.IServices
{
    public interface IWorkLogService
    {
        Task<IEnumerable<WorkLogDto>> GetAllWorkLogsForEmployeeAsync(Guid employeeId, bool trackChanges);
        Task<WorkLogDto> GetWorkLogForEmployeeAsync(Guid employeeId, Guid Id, bool trackChanges);
        Task<WorkLogDto> CreateWorkLogForEmployeeAsync(Guid employeeId, WorkLogForCreationDto workLog, bool trackChanges);
        Task DeleteWorkLogForEmployeeAsync(Guid employeeId, Guid Id, bool trackChanges);
        Task UpdateWorkLogAsync(Guid employeeId, Guid Id, WorkLogForUpdateDto workLogForUpdate, bool empTrackChanges, bool wrkTrackChanges);
    }
}