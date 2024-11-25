using AutoMapper;
using Cinema.Application.Queries.EventsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.EventsHandlers
{
    public sealed class GetEventsHandler : IRequestHandler<GetEventsQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetEventsHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            if (!request.EventParameters.ValidTicketPriceRange)
                return new MaxTicketPriceBadRequestPesponse();
            if (!request.EventParameters.ValidTimeRange)
                return new TimeRangeBadRequestResponse();

            var eventsWithMetaData = await _repository.Event.GetAllEventsAsync(request.EventParameters, request.TrackChanges);
            var eventsDto = _mapper.Map<IEnumerable<EventDto>>(eventsWithMetaData);

            return new ApiOkResponse<(IEnumerable<EventDto>, MetaData)>((eventsDto, eventsWithMetaData.MetaData));
        }
    }
}