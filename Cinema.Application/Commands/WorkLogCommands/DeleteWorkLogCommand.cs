using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.WorkLogCommands
{
    public sealed record DeleteWorkLogCommand(Guid EmployeeId, Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}