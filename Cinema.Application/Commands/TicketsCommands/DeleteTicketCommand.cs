using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.TicketsCommands
{
    public sealed record DeleteTicketCommand(Guid SeatId, Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}