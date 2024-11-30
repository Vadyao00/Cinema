using AutoMapper;
using Cinema.Application.Commands.WorkLogCommands;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.WorkLogsHandlers
{
    public sealed class CreateWorkLogHandler : IRequestHandler<CreateWorkLogCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateWorkLogHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(CreateWorkLogCommand request, CancellationToken cancellationToken)
        {
            var employeee = await _repository.Employee.GetEmployeeAsync(request.EmployeeId, request.TrackChanges);
            if (employeee is null)
                return new EmployeeNotFoundResponse(request.EmployeeId);
            var workLogDb = _mapper.Map<WorkLog>(request.WorkLog);

            _repository.WorkLog.CreateWorkLogForEmployee(request.EmployeeId, workLogDb);
            await _repository.SaveAsync();

            workLogDb.Employee = employeee;

            var workLogToReturn = _mapper.Map<WorkLogDto>(workLogDb);
            return new ApiOkResponse<WorkLogDto>(workLogToReturn);
        }
    }
}