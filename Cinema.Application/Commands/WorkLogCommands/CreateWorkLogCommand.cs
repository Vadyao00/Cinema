using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.WorkLogCommands
{
    public sealed record CreateWorkLogCommand(Guid EmployeeId, WorkLogForCreationDto WorkLog, bool TrackChanges) : IRequest<ApiBaseResponse>;
}