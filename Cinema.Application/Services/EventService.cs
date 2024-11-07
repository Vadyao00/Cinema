using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Cinema.LoggerService;
using Contracts.IRepositories;
using Contracts.IServices;

namespace Cinema.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EventService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<EventDto> CreateEventAsync(EventForCreationDto eevent)
        {
            var eventEntity = _mapper.Map<Event>(eevent);

            _repository.Event.CreateEvent(eventEntity);
            await _repository.SaveAsync();

            var eventToReturn = _mapper.Map<EventDto>(eventEntity);

            return eventToReturn;
        }

        public async Task<ApiBaseResponse> DeleteEventAsync(Guid eventId, bool trackChanges)
        {
            var eevent = await _repository.Event.GetEventAsync(eventId, trackChanges);
            if (eevent is null)
                return new EventNotFoundResponse(eventId);

            _repository.Event.DeleteEvent(eevent);
            await _repository.SaveAsync();

            return new ApiOkResponse<Event>(eevent);
        }

        public async Task<ApiBaseResponse> GetAllEventsAsync(bool trackChanges)
        {
            var events = await _repository.Event.GetAllEventsAsync(trackChanges);
            var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);

            return new ApiOkResponse<IEnumerable<EventDto>>(eventsDto);
        }

        public async Task<ApiBaseResponse> GetEventAsync(Guid eventId, bool trackChanges)
        {
            var eevent = await _repository.Event.GetEventAsync(eventId, trackChanges);
            if (eevent is null)
                return new EventNotFoundResponse(eventId);

            var eventDto = _mapper.Map<EventDto>(eevent);

            return new ApiOkResponse<EventDto>(eventDto);
        }

        public async Task<ApiBaseResponse> UpdateEventAsync(Guid eventId, EventForUpdateDto eventForUpdate, bool trackChanges)
        {
            var eevent = await _repository.Event.GetEventAsync(eventId, trackChanges);
            if (eevent is null)
                return new EventNotFoundResponse(eventId);

            _mapper.Map(eventForUpdate, eevent);
            await _repository.SaveAsync();

            return new ApiOkResponse<Event>(eevent);
        }
    }
}