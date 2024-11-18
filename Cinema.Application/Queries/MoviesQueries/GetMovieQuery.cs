using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.MoviesQueries
{
    public sealed record GetMovieQuery(Guid GenreId, Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}