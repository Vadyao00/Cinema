using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Exceptions;
using Cinema.LoggerService;
using Contracts.IRepositories;
using Contracts.IServices;
using System.Net.Sockets;

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

        public async Task<WorkLogDto> CreateWorkLogForEmployeeAsync(Guid employeeId, WorkLogForCreationDto workLog, bool trackChanges)
        {
            await CheckIfEmployeeExists(employeeId, trackChanges);

            var workLogDb = _mapper.Map<WorkLog>(workLog);

            _repository.WorkLog.CreateWorkLogForEmployee(employeeId, workLogDb);
            await _repository.SaveAsync();

            var employee = await GetEmployeeModel(employeeId, trackChanges: false);
            workLogDb.Employee = employee;

            var workLogToReturn = _mapper.Map<WorkLogDto>(workLogDb);
            return workLogToReturn;
        }

        public async Task DeleteWorkLogForEmployeeAsync(Guid employeeId, Guid Id, bool trackChanges)
        {
            var workLog = await GetWorkLogForEmployeeAndCheckIfItExists(employeeId, Id, trackChanges);

            _repository.WorkLog.DeleteWorkLog(workLog);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<WorkLogDto>> GetAllWorkLogsForEmployeeAsync(Guid employeeId, bool trackChanges)
        {
            var workLogs = await _repository.WorkLog.GetAllWorkLogsForEmployeeAsync(employeeId, trackChanges);
            var workLogsDto = _mapper.Map<IEnumerable<WorkLogDto>>(workLogs);

            return workLogsDto;
        }

        public async Task<WorkLogDto> GetWorkLogForEmployeeAsync(Guid employeeId, Guid Id, bool trackChanges)
        {
            var workLogDb = await GetWorkLogForEmployeeAndCheckIfItExists(employeeId, Id, trackChanges);

            var workLogDto = _mapper.Map<WorkLogDto>(workLogDb);
            return workLogDto;
        }

        public async Task UpdateWorkLogAsync(Guid employeeId, Guid Id, WorkLogForUpdateDto workLogForUpdate, bool empTrackChanges, bool wrkTrackChanges)
        {
            await CheckIfEmployeeExists(employeeId, empTrackChanges);

            var workLogEntity = await GetWorkLogForEmployeeAndCheckIfItExists(employeeId, Id, wrkTrackChanges);

            _mapper.Map(workLogForUpdate, workLogEntity);
            await _repository.SaveAsync();
        }

        private async Task CheckIfEmployeeExists(Guid employeeId, bool trackChanges)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(employeeId, trackChanges);
            if (employee is null)
                throw new EmployeeNotFoundException(employeeId);
        }

        private async Task<Employee> GetEmployeeModel(Guid employeeId, bool trackChanges)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(employeeId, trackChanges);
            if (employee is null)
                throw new EmployeeNotFoundException(employeeId);

            return employee;
        }

        private async Task<WorkLog> GetWorkLogForEmployeeAndCheckIfItExists(Guid employeeId, Guid id, bool trackChanges)
        {
            var workLogDb = await _repository.WorkLog.GetWorkLogForEmployeeAsync(employeeId, id, trackChanges);
            if (workLogDb is null)
                throw new WorkLogNotFoundException(id);

            return workLogDb;
        }
    }
}