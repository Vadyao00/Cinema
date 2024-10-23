using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class WorkLogRepository(CinemaContext dbContext) : RepositoryBase<WorkLog>(dbContext), IWorkLogRepository
    {
        public void CreateWorkLog(WorkLog workLog) => Create(workLog);

        public void DeleteWorkLog(WorkLog workLog) => Delete(workLog);

        public async Task<IEnumerable<WorkLog>> GetAllWorkLogsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                  .OrderBy(w => w.WorkHours)
                  .ToListAsync();

        public async Task<IEnumerable<WorkLog>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(w => ids.Contains(w.WorkLogId), trackChanges)
                  .ToListAsync();

        public async Task<WorkLog> GetWorkLogsAsync(Guid id, bool trackChanges) =>
            await FindByCondition(w => w.WorkLogId.Equals(id), trackChanges)
                  .SingleOrDefaultAsync();
    }
}
