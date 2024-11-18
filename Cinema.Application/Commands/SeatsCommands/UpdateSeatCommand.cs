using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.SeatsCommands
{
    public sealed record UpdateSeatCommand(Guid Id, SeatForUpdateDto SeatForUpdate, bool SeatTrackChanges) : IRequest<ApiBaseResponse>;
}