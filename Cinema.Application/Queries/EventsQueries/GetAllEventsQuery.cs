using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.EventsQueries
{
    public sealed record GetAllEventsQuery(bool TrackChanges) : IRequest<ApiBaseResponse>;
}