using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IWorkLogRepository
    {
        Task<IEnumerable<WorkLog>> GetAllWorkLogsForEmployeeAsync(Guid employee, bool trackChanges);
        Task<WorkLog> GetWorkLogForEmployeeAsync(Guid employeeId, Guid id, bool trackChanges);
        void CreateWorkLogForEmployee(Guid employeeId, WorkLog workLog);
        Task<IEnumerable<WorkLog>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteWorkLog(WorkLog workLog);
    }
}