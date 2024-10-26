using Cinema.Domain.DataTransferObjects;

namespace Contracts.IServices
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetAllEventsAsync(bool trackChanges);
        Task<EventDto> GetEventAsync(Guid eventId, bool trackChanges);
        Task<EventDto> CreateEventAsync(EventForCreationDto eevent);
        Task<IEnumerable<EventDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        Task DeleteEventAsync(Guid eventId, bool trackChanges);
        Task UpdateEventAsync(Guid eventId, EventForUpdateDto eventForUpdate, bool trackChanges);
    }
}
