using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.ActorsCommands
{
    public record DeleteActorCommand(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}