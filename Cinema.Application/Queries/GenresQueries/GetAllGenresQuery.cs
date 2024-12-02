using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.GenresQueries
{
    public sealed record GetAllGenresQuery(bool TrackChanges) : IRequest<ApiBaseResponse>;
}