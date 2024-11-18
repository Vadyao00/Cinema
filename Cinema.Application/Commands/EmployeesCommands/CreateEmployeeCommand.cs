using Cinema.Domain.DataTransferObjects;
using MediatR;

namespace Cinema.Application.Commands.EmployeesCommands
{
    public sealed record CreateEmployeeCommand(EmployeeForCreationDto EmployeeDto) : IRequest<EmployeeDto>;
}