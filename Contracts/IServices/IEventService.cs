using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;

namespace Contracts.IServices
{
    public interface IEventService
    {
        Task<ApiBaseResponse> GetAllEventsAsync(EventParameters eventParameters, bool trackChanges);
        Task<ApiBaseResponse> GetEventAsync(Guid eventId, bool trackChanges);
        Task<EventDto> CreateEventAsync(EventForCreationDto eevent);
        Task<ApiBaseResponse> DeleteEventAsync(Guid eventId, bool trackChanges);
        Task<ApiBaseResponse> UpdateEventAsync(Guid eventId, EventForUpdateDto eventForUpdate, bool trackChanges);
    }
}