using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.ActorsQueries
{
    public sealed record GetAllActorsQuery(bool TrackChanges) : IRequest<ApiBaseResponse>;
}