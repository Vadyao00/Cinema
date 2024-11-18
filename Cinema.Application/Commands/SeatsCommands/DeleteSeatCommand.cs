using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.SeatsCommands
{
    public sealed record DeleteSeatCommand(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}