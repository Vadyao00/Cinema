using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.EventsQueries
{
    public sealed record GetEventQuery(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}