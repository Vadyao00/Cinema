using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.SeatsCommands
{
    public sealed record CreateSeatCommand(Guid? ShowtimeId, Guid? EventId, SeatForCreationDto Seat, bool TrackChanges) : IRequest<ApiBaseResponse>;
}