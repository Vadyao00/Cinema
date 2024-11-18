using Cinema.Domain.DataTransferObjects;
using MediatR;

namespace Cinema.Application.Commands.EventsCommands
{
    public sealed record CreateEventCommand(EventForCreationDto EventDto) : IRequest<EventDto>;
}