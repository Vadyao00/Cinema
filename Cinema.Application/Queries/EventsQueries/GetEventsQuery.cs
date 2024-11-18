using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.EventsQueries
{
    public sealed record GetEventsQuery(EventParameters EventParameters, bool TrackChanges) : IRequest<ApiBaseResponse>;
}