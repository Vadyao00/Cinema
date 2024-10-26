using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IWorkLogRepository
    {
        Task<IEnumerable<WorkLog>> GetAllWorkLogsAsync(bool trackChanges);
        Task<WorkLog> GetWorkLogAsync(Guid id, bool trackChanges);
        void CreateWorkLogForEmployee(Guid employeeId, WorkLog workLog);
        Task<IEnumerable<WorkLog>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteWorkLog(WorkLog workLog);
    }
}