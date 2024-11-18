using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Persistence.Extensions;
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

        public async Task<PagedList<WorkLog>> GetAllWorkLogsAsync(WorkLogParameters workLogParameters, bool trackChanges)
        {
            var workLogs = await FindAll(trackChanges)
                  .Search(workLogParameters.searchName)
                  .Include(w => w.Employee)
                  .Sort(workLogParameters.OrderBy)
                  .Skip((workLogParameters.PageNumber - 1) * workLogParameters.PageSize)
                  .Take(workLogParameters.PageSize)
                  .ToListAsync();

            var count = await FindAll(trackChanges).CountAsync();

            return new PagedList<WorkLog>(workLogs, count, workLogParameters.PageNumber, workLogParameters.PageSize);
        }

        public async Task<PagedList<WorkLog>> GetAllWorkLogsForEmployeeAsync(WorkLogParameters workLogParameters, Guid employeeId, bool trackChanges)
        {
            var workLogs = await FindByCondition(w => w.EmployeeId.Equals(employeeId), trackChanges)
                  .Search(workLogParameters.searchName)
                  .Include(w => w.Employee)
                  .Sort(workLogParameters.OrderBy)
                  .Skip((workLogParameters.PageNumber - 1) * workLogParameters.PageSize)
                  .Take(workLogParameters.PageSize)
                  .ToListAsync();

            var count = await FindByCondition(w => w.EmployeeId.Equals(employeeId), trackChanges).CountAsync();

            return new PagedList<WorkLog>(workLogs, count, workLogParameters.PageNumber, workLogParameters.PageSize);
        }

        public async Task<WorkLog> GetWorkLogForEmployeeAsync(Guid employeeId, Guid id, bool trackChanges) =>
            await FindByCondition(w => w.WorkLogId.Equals(id) && w.EmployeeId.Equals(employeeId), trackChanges)
                  .Include(w => w.Employee)
                  .SingleOrDefaultAsync();
    }
}