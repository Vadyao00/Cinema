using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.TicketsCommands
{
    public sealed record DeleteTicketCommand(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}