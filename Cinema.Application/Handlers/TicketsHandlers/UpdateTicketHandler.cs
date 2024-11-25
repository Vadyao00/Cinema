using AutoMapper;
using Cinema.Application.Commands.TicketsCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.TicketsHandlers
{
    public sealed class UpdateTicketHandler : IRequestHandler<UpdateTicketCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateTicketHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            var seat = await _repository.Seat.GetSeatAsync(request.SeatId, request.SeatTrackChanges);
            if (seat is null)
                return new SeatNotFoundResponse(request.SeatId);

            var ticketEntity = await _repository.Ticket.GetTicketAsync(request.Id, request.TickTrackChanges);
            if (ticketEntity is null)
                return new TicketNotFoundResponse(request.Id);

            _mapper.Map(request.TicketForUpdate, ticketEntity);
            await _repository.SaveAsync();

            return new ApiOkResponse<Ticket>(ticketEntity);
        }
    }
}