using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.TicketsQueries
{
    public sealed record GetTicketsQuery(TicketParameters TicketParameters, Guid SeatId, bool TrackChanges) : IRequest<ApiBaseResponse>;
}