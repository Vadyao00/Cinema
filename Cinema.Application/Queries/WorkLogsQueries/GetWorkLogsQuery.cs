﻿using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using MediatR;

namespace Cinema.Application.Queries.WorkLogsQueries
{
    public sealed record GetWorkLogsQuery(WorkLogParameters WorkLogParameters, bool TrackChanges) : IRequest<ApiBaseResponse>;
}