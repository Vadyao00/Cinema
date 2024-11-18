using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.SeatsQueries
{
    public sealed record GetSeatQuery(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}