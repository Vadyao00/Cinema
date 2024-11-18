using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.GenresQueries
{
    public sealed record GetGenreQuery(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}