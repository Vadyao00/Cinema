using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.GenresCommands
{
    public sealed record UpdateGenreCommand(Guid Id, GenreForUpdateDto GenreForUpdateDto, bool TrackChanges) : IRequest<ApiBaseResponse>;
}