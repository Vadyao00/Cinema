using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.ShowtimesCommands
{
    public sealed record CreateShowtimeCommand(Guid GenreId, Guid MovieId, ShowtimeForCreationDto Showtime, bool TrackChanges) : IRequest<ApiBaseResponse>;
}