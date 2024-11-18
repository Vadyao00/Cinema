using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.WorkLogCommands
{
    public sealed record UpdateWorkLogCommand(Guid EmployeeId, Guid Id, WorkLogForUpdateDto WorkLogForUpdate, bool EmpTrackChanges, bool WrkTrackChanges) : IRequest<ApiBaseResponse>;
}