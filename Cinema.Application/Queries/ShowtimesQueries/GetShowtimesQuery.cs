using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.ShowtimesQueries
{
    public sealed record GetShowtimesQuery(ShowtimeParameters ShowtimeParameters, bool TrackChanges) : IRequest<ApiBaseResponse>;
}