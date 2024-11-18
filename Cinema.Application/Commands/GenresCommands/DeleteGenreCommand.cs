using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.GenresCommands
{
    public sealed record DeleteGenreCommand(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}