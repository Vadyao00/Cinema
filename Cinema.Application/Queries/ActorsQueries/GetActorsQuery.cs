using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.ActorsQueries
{
    public sealed record GetActorsQuery(ActorParameters ActorParameters, bool TrackChanges) : IRequest<ApiBaseResponse>;
}