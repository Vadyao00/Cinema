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

        public async Task<ApiBaseResponse> DeleteEmployeeAsync(Guid employeeId, bool trackChanges)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(employeeId, trackChanges);

            if (employee is null)
                return new EmployeeNotFoundResponse(employeeId);

            _repository.Employee.DeleteEmployee(employee);
            await _repository.SaveAsync();

            return new ApiOkResponse<Employee>(employee);
        }

        public async Task<ApiBaseResponse> GetAllEmployeesAsync(EmployeeParameters employeeParameters, bool trackChanges)
        {
            var employeesWithMetaData = await _repository.Employee.GetEmployeesAsync(employeeParameters, trackChanges);
            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);

            return new ApiOkResponse<(IEnumerable<EmployeeDto>, MetaData)>((employeesDto, employeesWithMetaData.MetaData));
        }

        public async Task<ApiBaseResponse> GetEmployeeAsync(Guid employeeId, bool trackChanges)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(employeeId, trackChanges);

            if (employee is null)
                return new EmployeeNotFoundResponse(employeeId);

            var employeeDto = _mapper.Map<EmployeeDto>(employee);

            return new ApiOkResponse<EmployeeDto>(employeeDto);
        }

        public async Task<ApiBaseResponse> UpdateEmployeeAsync(Guid employeeId, EmployeeForUpdateDto employeeForUpdate, bool trackChanges)
        {
            var employee = await _repository.Employee.GetEmployeeAsync(employeeId, trackChanges);

            if (employee is null)
                return new EmployeeNotFoundResponse(employeeId);

            _mapper.Map(employeeForUpdate, employee);
            await _repository.SaveAsync();

            return new ApiOkResponse<Employee>(employee);
        }
    }
}