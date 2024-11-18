using AutoMapper;
using Cinema.Application.Commands.TicketsCommands;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.TicketsHandlers
{
    internal sealed class CreateTicketHandler : IRequestHandler<CreateTicketCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateTicketHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            var ticketDb = _mapper.Map<Ticket>(request.Ticket);

            _repository.Ticket.CreateTicketForSeat(request.SeatId, ticketDb);
            await _repository.SaveAsync();

            var seat = await _repository.Seat.GetSeatAsync(request.SeatId, request.TrackChanges);
            if (seat is null)
                return new SeatNotFoundResponse(request.SeatId);
            ticketDb.Seat = seat;

            var ticketToReturn = _mapper.Map<TicketDto>(ticketDb);
            return new ApiOkResponse<TicketDto>(ticketToReturn);
        }
    }
}