using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;

namespace Contracts.IRepositories
{
    public interface IWorkLogRepository
    {
        Task<PagedList<WorkLog>> GetAllWorkLogsForEmployeeAsync(WorkLogParameters workLogParameters, Guid employee, bool trackChanges);
        Task<PagedList<WorkLog>> GetAllWorkLogsAsync(WorkLogParameters workLogParameters, bool trackChanges);
        Task<WorkLog> GetWorkLogForEmployeeAsync(Guid employeeId, Guid id, bool trackChanges);
        void CreateWorkLogForEmployee(Guid employeeId, WorkLog workLog);
        void DeleteWorkLog(WorkLog workLog);
    }
}