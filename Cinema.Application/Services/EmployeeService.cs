using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Exceptions;
using Cinema.LoggerService;
using Contracts.IRepositories;
using Contracts.IServices;

namespace Cinema.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EmployeeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<EmployeeDto> CreateEmployeeAsync(EmployeeForCreationDto employee)
        {
            var employeeEntity = _mapper.Map<Employee>(employee);

            _repository.Employee.CreateEmployee(employeeEntity);
            await _repository.SaveAsync();

            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return employeeToReturn;
        }

        public async Task DeleteEmployeeAsync(Guid employeeId, bool trackChanges)
        {
            var employee = await GetEmployeeAndCheckIfItExists(employeeId, trackChanges);

            _repository.Employee.DeleteEmployee(employee);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(bool trackChanges)
        {
            var employees = await _repository.Employee.GetEmployeesAsync(trackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeesDto;

        }

        public async Task<IEnumerable<EmployeeDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if(ids is null)
                throw new IdParametrBadRequestException();

            var employeeEntities = await _repository.Employee.GetByIdsAsync(ids, trackChanges);
            if(ids.Count() != employeeEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var employeesToReturn = _mapper.Map<IEnumerable<EmployeeDto>>(employeeEntities);

            return employeesToReturn;
        }

        public async Task<EmployeeDto> GetEmployeeAsync(Guid employeeId, bool trackChanges)
        {
            var employee = await GetEmployeeAndCheckIfItExists(employeeId, trackChanges);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public async Task UpdateEmployeeAsync(Guid employeeId, EmployeeForUpdateDto employeeForUpdate, bool trackChanges)
        {
            var employeeEntity = await GetEmployeeAndCheckIfItExists(employeeId, trackChanges);

            _mapper.Map(employeeForUpdate, employeeEntity);
            await _repository.SaveAsync();
        }

        private async Task<Employee> GetEmployeeAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(id, trackChanges);
            if(employee is null)
                throw new EmployeeNotFoundException(id);
            return employee;
        }
    }
}