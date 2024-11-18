using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.EventsCommands
{
    public sealed record UpdateEventCommand(Guid Id, EventForUpdateDto EventForUpdateDto, bool TrackChanges) : IRequest<ApiBaseResponse>;
}