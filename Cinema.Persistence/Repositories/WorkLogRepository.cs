using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class WorkLogRepository(CinemaContext dbContext) : RepositoryBase<WorkLog>(dbContext), IWorkLogRepository
    {
        public void CreateWorkLogForEmployee(Guid employeeId, WorkLog workLog)
        {
            workLog.EmployeeId = employeeId;
            Create(workLog);
        }

        public void DeleteWorkLog(WorkLog workLog) => Delete(workLog);

        public async Task<IEnumerable<WorkLog>> GetAllWorkLogsForEmployeeAsync(Guid employeeId, bool trackChanges) =>
            await FindByCondition(w => w.EmployeeId.Equals(employeeId), trackChanges)
                  .Include(w => w.Employee)
                  .OrderBy(w => w.WorkHours)
                  .ToListAsync();

        public async Task<IEnumerable<WorkLog>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(w => ids.Contains(w.WorkLogId), trackChanges)
                  .ToListAsync();

        public async Task<WorkLog> GetWorkLogForEmployeeAsync(Guid employeeId, Guid id, bool trackChanges) =>
            await FindByCondition(w => w.WorkLogId.Equals(id) && w.EmployeeId.Equals(employeeId), trackChanges)
                  .Include(w => w.Employee)
                  .SingleOrDefaultAsync();
    }
}
