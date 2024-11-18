using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.EmployeesQueries
{
    public sealed record GetEmployeeQuery(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}