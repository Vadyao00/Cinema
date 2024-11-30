using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.WorkLogCommands
{
    public sealed record DeleteWorkLogCommand(Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}