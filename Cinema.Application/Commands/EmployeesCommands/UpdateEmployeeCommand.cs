using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.EmployeesCommands
{
    public sealed record UpdateEmployeeCommand(Guid Id, EmployeeForUpdateDto EmployeeForUpdateDto, bool TrackChanges) : IRequest<ApiBaseResponse>;
}