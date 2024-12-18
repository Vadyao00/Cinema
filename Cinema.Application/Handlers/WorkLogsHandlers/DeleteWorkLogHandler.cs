﻿using AutoMapper;
using Cinema.Application.Commands.WorkLogCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.WorkLogsHandlers
{
    public sealed class DeleteWorkLogHandler : IRequestHandler<DeleteWorkLogCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public DeleteWorkLogHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(DeleteWorkLogCommand request, CancellationToken cancellationToken)
        {
            var workLog = await _repository.WorkLog.GetWorkLogAsync(request.Id, request.TrackChanges);
            if (workLog is null)
                return new WorkLogNotFoundResponse(request.Id);

            _repository.WorkLog.DeleteWorkLog(workLog);
            await _repository.SaveAsync();

            return new ApiOkResponse<WorkLog>(workLog);
        }
    }
}