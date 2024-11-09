using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Cinema.LoggerService;
using Contracts.IRepositories;
using Contracts.IServices;

namespace Cinema.Application.Services
{
    public class WorkLogService : IWorkLogService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public WorkLogService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> CreateWorkLogForEmployeeAsync(Guid employeeId, WorkLogForCreationDto workLog, bool trackChanges)
        {
            var employeee = await _repository.Employee.GetEmployeeAsync(employeeId, trackChanges);
            if (employeee is null)
                return new EmployeeNotFoundResponse(employeeId);

            var workLogDb = _mapper.Map<WorkLog>(workLog);

            _repository.WorkLog.CreateWorkLogForEmployee(employeeId, workLogDb);
            await _repository.SaveAsync();

            var employee = await _repository.Employee.GetEmployeeAsync(employeeId, trackChanges);
            if (employee is null)
                return new EmployeeNotFoundResponse(employeeId);
            workLogDb.Employee = employee;

            var workLogToReturn = _mapper.Map<WorkLogDto>(workLogDb);
            return new ApiOkResponse<WorkLogDto>(workLogToReturn);
        }

        public async Task<ApiBaseResponse> DeleteWorkLogForEmployeeAsync(Guid employeeId, Guid Id, bool trackChanges)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(employeeId, trackChanges);
            if (employee is null)
                return new EmployeeNotFoundResponse(employeeId);

            var workLog = await _repository.WorkLog.GetWorkLogForEmployeeAsync(employeeId, Id, trackChanges);
            if (workLog is null)
                return new WorkLogNotFoundResponse(Id);

            _repository.WorkLog.DeleteWorkLog(workLog);
            await _repository.SaveAsync();

            return new ApiOkResponse<WorkLog>(workLog);
        }

        public async Task<ApiBaseResponse> GetAllWorkLogsForEmployeeAsync(WorkLogParameters workLogParameters, Guid employeeId, bool trackChanges)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(employeeId, trackChanges);
            if (employee is null)
                return new EmployeeNotFoundResponse(employeeId);

            var workLogsWithMetaData = await _repository.WorkLog.GetAllWorkLogsForEmployeeAsync(workLogParameters, employeeId, trackChanges);
            var workLogsDto = _mapper.Map<IEnumerable<WorkLogDto>>(workLogsWithMetaData);

            return new ApiOkResponse<(IEnumerable<WorkLogDto>, MetaData)>((workLogsDto, workLogsWithMetaData.MetaData));
        }

        public async Task<ApiBaseResponse> GetWorkLogForEmployeeAsync(Guid employeeId, Guid Id, bool trackChanges)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(employeeId, trackChanges);
            if (employee is null)
                return new EmployeeNotFoundResponse(employeeId);

            var workLogDb = await _repository.WorkLog.GetWorkLogForEmployeeAsync(employeeId, Id, trackChanges);
            if (workLogDb is null)
                return new WorkLogNotFoundResponse(Id);

            var workLogDto = _mapper.Map<WorkLogDto>(workLogDb);
            return new ApiOkResponse<WorkLogDto>(workLogDto);
        }

        public async Task<ApiBaseResponse> UpdateWorkLogAsync(Guid employeeId, Guid Id, WorkLogForUpdateDto workLogForUpdate, bool empTrackChanges, bool wrkTrackChanges)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(employeeId, empTrackChanges);
            if (employee is null)
                return new EmployeeNotFoundResponse(employeeId);

            var workLogEntity = await _repository.WorkLog.GetWorkLogForEmployeeAsync(employeeId, Id, wrkTrackChanges);
            if (workLogEntity is null)
                return new WorkLogNotFoundResponse(Id);

            _mapper.Map(workLogForUpdate, workLogEntity);
            await _repository.SaveAsync();

            return new ApiOkResponse<WorkLog>(workLogEntity);
        }
    }
}