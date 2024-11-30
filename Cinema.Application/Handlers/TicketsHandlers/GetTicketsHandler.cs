using AutoMapper;
using Cinema.Application.Queries.TicketsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.TicketsHandlers
{
    public sealed class GetTicketsHandler : IRequestHandler<GetTicketsQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetTicketsHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
        {
            if (!request.TicketParameters.ValidSeatNumber)
                return new SeatNumberBadRequestResponse();

            var ticketsWithMetaData = await _repository.Ticket.GetAllTicketsAsync(request.TicketParameters, request.TrackChanges);
            var ticketsDto = _mapper.Map<IEnumerable<TicketDto>>(ticketsWithMetaData);

            return new ApiOkResponse<(IEnumerable<TicketDto>, MetaData)>((ticketsDto, ticketsWithMetaData.MetaData));
        }
    }
}