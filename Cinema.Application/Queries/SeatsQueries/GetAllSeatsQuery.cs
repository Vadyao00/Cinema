using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.SeatsQueries
{
    public sealed record GetAllSeatsQuery(bool TrackChanges) : IRequest<ApiBaseResponse>;
}