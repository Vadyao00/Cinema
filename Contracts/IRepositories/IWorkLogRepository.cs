using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IWorkLogRepository
    {
        Task<IEnumerable<WorkLog>> GetAllWorkLogsAsync(bool trackChanges);
        Task<WorkLog> GetWorkLogsAsync(Guid id, bool trackChanges);
        void CreateWorkLog(WorkLog workLog);
        Task<IEnumerable<WorkLog>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteWorkLog(WorkLog workLog);
    }
}