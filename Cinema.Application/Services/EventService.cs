using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Exceptions;
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

        public async Task DeleteEventAsync(Guid eventId, bool trackChanges)
        {
            var eevent = await GetEventAndCheckIfItExists(eventId, trackChanges);

            _repository.Event.DeleteEvent(eevent);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync(bool trackChanges)
        {
            var events = await _repository.Event.GetAllEventsAsync(trackChanges);
            var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);

            return eventsDto;
        }

        public async Task<IEnumerable<EventDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if(ids is null)
                throw new IdParametrBadRequestException();

            var eventEntities = await _repository.Event.GetByIdsAsync(ids, trackChanges);
            if(ids.Count() != eventEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var eventsToReturn = _mapper.Map<IEnumerable<EventDto>>(eventEntities);

            return eventsToReturn;
        }

        public async Task<EventDto> GetEventAsync(Guid eventId, bool trackChanges)
        {
            var eevent = await GetEventAndCheckIfItExists(eventId, trackChanges);

            var eventDto = _mapper.Map<EventDto>(eevent);

            return eventDto;

        }

        public async Task UpdateEventAsync(Guid eventId, EventForUpdateDto eventForUpdate, bool trackChanges)
        {
            var eventEntity = await GetEventAndCheckIfItExists(eventId, trackChanges);

            _mapper.Map(eventForUpdate, eventEntity);
            await _repository.SaveAsync();
        }

        private async Task<Event> GetEventAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var eevent = await _repository.Event.GetEventsAsync(id, trackChanges);
            if (eevent is null)
                throw new EventNotFoundException(id);
            return eevent;
        }
    }
}