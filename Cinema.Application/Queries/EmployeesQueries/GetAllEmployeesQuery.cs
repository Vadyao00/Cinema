using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.EmployeesQueries
{
    public sealed record GetAllEmployeesQuery(bool TrackChanges) : IRequest<ApiBaseResponse>;
}