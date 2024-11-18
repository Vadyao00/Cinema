﻿using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.MoviesQueries
{
    public sealed record GetMoviesQuery(MovieParameters MovieParameters, Guid GenreId, bool TrackChanges) : IRequest<ApiBaseResponse>;
}