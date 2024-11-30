using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.TicketsQueries
{
    public sealed record GetTicketQuery(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}