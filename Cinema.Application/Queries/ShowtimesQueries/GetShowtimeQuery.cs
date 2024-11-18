using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.ShowtimesQueries
{
    public sealed record GetShowtimeQuery(Guid GenreId, Guid MovieId, Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}