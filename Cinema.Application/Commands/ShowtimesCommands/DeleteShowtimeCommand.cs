using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.ShowtimesCommands
{
    public sealed record DeleteShowtimeCommand(Guid GenreId, Guid MovieId, Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}