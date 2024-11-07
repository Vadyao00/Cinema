using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEventsAsync(bool trackChanges);
        Task<Event> GetEventAsync(Guid id, bool trackChanges);
        void CreateEvent(Event eevent);
        void DeleteEvent(Event eevent);
    }
}
