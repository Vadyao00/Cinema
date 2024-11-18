using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.GenresCommands
{
    public sealed record CreateGenreCommand(GenreForCreationDto GenreDto) : IRequest<GenreDto>;
}