using AutoMapper;
using Cinema.Application.Queries.EventsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.EventsHandlers
{
    internal sealed class GetEventHandler : IRequestHandler<GetEventQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetEventHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetEventQuery request, CancellationToken cancellationToken)
        {
            var eevent = await _repository.Event.GetEventAsync(request.Id, request.TrackChanges);
            if (eevent is null)
                return new EventNotFoundResponse(request.Id);

            var eventDto = _mapper.Map<EventDto>(eevent);

            return new ApiOkResponse<EventDto>(eventDto);
        }
    }
}