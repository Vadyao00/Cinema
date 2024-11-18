using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.MoviesCommands
{
    public sealed record UpdateMovieCommand(Guid GenreId, Guid Id, MovieForUpdateDto MovieForUpdate, bool GenrTrackChanges, bool MovTrackChanges) : IRequest<ApiBaseResponse>;
}