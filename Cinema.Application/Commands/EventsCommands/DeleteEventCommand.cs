using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.EventsCommands
{
    public sealed record DeleteEventCommand(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}