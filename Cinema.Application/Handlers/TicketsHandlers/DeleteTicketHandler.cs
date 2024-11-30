using AutoMapper;
using Cinema.Application.Commands.TicketsCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.TicketsHandlers
{
    public sealed class DeleteTicketHandler : IRequestHandler<DeleteTicketCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public DeleteTicketHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _repository.Ticket.GetTicketAsync(request.Id, request.TrackChanges);
            if (ticket is null)
                return new TicketNotFoundResponse(request.Id);

            _repository.Ticket.DeleteTicket(ticket);
            await _repository.SaveAsync();

            return new ApiOkResponse<Ticket>(ticket);
        }
    }
}