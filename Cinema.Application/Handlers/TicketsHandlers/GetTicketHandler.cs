using AutoMapper;
using Cinema.Application.Queries.TicketsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.TicketsHandlers
{
    public sealed class GetTicketHandler : IRequestHandler<GetTicketQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetTicketHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetTicketQuery request, CancellationToken cancellationToken)
        {
            var ticketDb = await _repository.Ticket.GetTicketAsync(request.Id, request.TrackChanges);
            if (ticketDb is null)
                return new TicketNotFoundResponse(request.Id);

            var ticketDto = _mapper.Map<TicketDto>(ticketDb);
            return new ApiOkResponse<TicketDto>(ticketDto);
        }
    }
}