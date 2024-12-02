using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.ShowtimesQueries
{
    public sealed record GetAllShowtimesQuery(bool TrackChanges) : IRequest<ApiBaseResponse>;
}