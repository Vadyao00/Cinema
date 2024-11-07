using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class EventRepository(CinemaContext dbContext) : RepositoryBase<Event>(dbContext), IEventRepository
    {
        public void CreateEvent(Event eevent) => Create(eevent);

        public void DeleteEvent(Event eevent) => Delete(eevent);

        public async Task<IEnumerable<Event>> GetAllEventsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                  .OrderBy(e => e.StartTime)
                  .ToListAsync();

        public async Task<Event> GetEventAsync(Guid id, bool trackChanges) =>
            await FindByCondition(e => e.EventId.Equals(id), trackChanges)
                  .SingleOrDefaultAsync();
    }
}
