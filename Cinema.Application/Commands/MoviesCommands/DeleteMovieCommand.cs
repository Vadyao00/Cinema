﻿using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.MoviesCommands
{
    public sealed record DeleteMovieCommand(Guid GenreId, Guid Id, bool TrackChanges) : IRequest<ApiBaseResponse>;
}