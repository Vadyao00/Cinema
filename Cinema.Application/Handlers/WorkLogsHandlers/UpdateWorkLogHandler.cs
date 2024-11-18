using AutoMapper;
using Cinema.Application.Commands.WorkLogCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.WorkLogsHandlers
{
    internal sealed class UpdateWorkLogHandler : IRequestHandler<UpdateWorkLogCommand, ApiBaseResponse>
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
            var employee = await _repository.Employee.GetEmployeeAsync(request.EmployeeId, request.EmpTrackChanges);
            if (employee is null)
                return new EmployeeNotFoundResponse(request.EmployeeId);

            var workLogEntity = await _repository.WorkLog.GetWorkLogForEmployeeAsync(request.EmployeeId, request.Id, request.WrkTrackChanges);
            if (workLogEntity is null)
                return new WorkLogNotFoundResponse(request.Id);

            _mapper.Map(request.WorkLogForUpdate, workLogEntity);
            await _repository.SaveAsync();

            return new ApiOkResponse<WorkLog>(workLogEntity);
        }
    }
}