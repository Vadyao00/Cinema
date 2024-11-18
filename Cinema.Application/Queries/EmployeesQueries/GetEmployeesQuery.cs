using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.EmployeesQueries
{
    public sealed record GetEmployeesQuery(EmployeeParameters EmployeeParameters, bool TrackChanges) : IRequest<ApiBaseResponse>;
}