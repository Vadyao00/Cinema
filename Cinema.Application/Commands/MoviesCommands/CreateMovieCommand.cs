using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.MoviesCommands
{
    public sealed record CreateMovieCommand(Guid GenreId, MovieForCreationDto MovieDto, bool TrackChanges) : IRequest<ApiBaseResponse>;
}