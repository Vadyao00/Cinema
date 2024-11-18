using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.SeatsQueries
{
    public sealed record GetSeatsQuery(SeatParameters SeatParameters, bool TrackChanges) : IRequest<ApiBaseResponse>;
}