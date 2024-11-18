using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.ActorsCommands
{
    public sealed record UpdateActorCommand(Guid Id, ActorForUpdateDto ActorForUpdateDto, bool TrackChanges) : IRequest<ApiBaseResponse>;
}