using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync(bool trackChanges);
        Task<Event> GetEventAsync(Guid id, bool trackChanges);
        void CreateEvent(Event eevent);
        Task<IEnumerable<Event>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteEvent(Event eevent);
    }
}
