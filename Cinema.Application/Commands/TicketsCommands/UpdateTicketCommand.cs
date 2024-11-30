using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.TicketsCommands
{
    public sealed record UpdateTicketCommand(Guid Id, TicketForUpdateDto TicketForUpdate, bool SeatTrackChanges, bool TickTrackChanges) : IRequest<ApiBaseResponse>;
}