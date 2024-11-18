using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.GenresQueries
{
    public sealed record GetGenresQuery(GenreParameters GenreParameters, bool TrackChanges) : IRequest<ApiBaseResponse>;
}