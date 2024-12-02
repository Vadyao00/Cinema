using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.MoviesQueries
{
    public sealed record GetAllMoviesQuery(bool TrackChanges) : IRequest<ApiBaseResponse>;
}