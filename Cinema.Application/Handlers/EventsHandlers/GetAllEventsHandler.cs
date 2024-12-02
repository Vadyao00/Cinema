using AutoMapper;
using Cinema.Application.Queries.EventsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.EventsHandlers
{
    public sealed class GetAllEventsHandler : IRequestHandler<GetAllEventsQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetAllEventsHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            var events = await _repository.Event.GetAllEventsWithoutMetaAsync(request.TrackChanges);
            var eventsDto = _mapper.Map<IEnumerable<EventDto>>(events);

            return new ApiOkResponse<IEnumerable<EventDto>>(eventsDto);
        }
    }
}