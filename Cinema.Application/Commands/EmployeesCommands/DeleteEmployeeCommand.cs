using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.EmployeesCommands
{
    public sealed record DeleteEmployeeCommand(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}