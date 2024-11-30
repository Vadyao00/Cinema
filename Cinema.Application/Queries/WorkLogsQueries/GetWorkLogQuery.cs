using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.WorkLogsQueries
{
    public sealed record GetWorkLogQuery(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}