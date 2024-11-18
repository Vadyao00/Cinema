using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.ActorsQueries
{
    public sealed record GetActorQuery(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}