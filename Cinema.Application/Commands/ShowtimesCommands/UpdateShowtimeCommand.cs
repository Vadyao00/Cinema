﻿using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Commands.ShowtimesCommands
{
    public sealed record UpdateShowtimeCommand(Guid Id, ShowtimeForUpdateDto ShowtimeForUpdate, bool MovTrackChanges, bool ShwTrackChanges) : IRequest<ApiBaseResponse>;
}