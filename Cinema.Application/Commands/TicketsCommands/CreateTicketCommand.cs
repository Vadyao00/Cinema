using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.TicketsCommands
{
    public sealed record CreateTicketCommand(Guid SeatId, TicketForCreationDto Ticket, bool TrackChanges) : IRequest<ApiBaseResponse>;
}