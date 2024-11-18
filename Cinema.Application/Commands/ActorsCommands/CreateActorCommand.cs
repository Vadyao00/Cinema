using Cinema.Domain.DataTransferObjects;
using MediatR;

namespace Cinema.Application.Commands.ActorsCommands
{
    public sealed record CreateActorCommand(ActorForCreationDto ActorDto) : IRequest<ActorDto>;
}