﻿using AutoMapper;
using Cinema.Application.Commands.WorkLogCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.WorkLogsHandlers
{
    public sealed class UpdateWorkLogHandler : IRequestHandler<UpdateWorkLogCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateWorkLogHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(UpdateWorkLogCommand request, CancellationToken cancellationToken)
        {
            var workLogEntity = await _repository.WorkLog.GetWorkLogAsync(request.Id, request.WrkTrackChanges);
            if (workLogEntity is null)
                return new WorkLogNotFoundResponse(request.Id);

            _mapper.Map(request.WorkLogForUpdate, workLogEntity);
            await _repository.SaveAsync();

            return new ApiOkResponse<WorkLog>(workLogEntity);
        }
    }
}