using Cinema.Domain.DataTransferObjects;

namespace Contracts.IServices
{
    public interface IWorkLogService
    {
        Task<IEnumerable<WorkLogDto>> GetAllWorkLogsAsync(Guid employeeId, bool trackChanges);
        Task<WorkLogDto> GetWorkLogAsync(Guid Id, bool trackChanges);
        Task<WorkLogDto> CreateWorkLogForEmployeeAsync(Guid employeeId, WorkLogForCreationDto workLog, bool trackChanges);
        Task DeleteWorkLogAsync(Guid Id, bool trackChanges);
        Task UpdateWorkLogAsync(Guid employeeId, Guid Id, WorkLogForUpdateDto workLogForUpdate, bool empTrackChanges, bool wrkTrackChanges);
    }
}
