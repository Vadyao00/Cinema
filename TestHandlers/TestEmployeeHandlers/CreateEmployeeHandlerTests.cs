using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Moq;
using Cinema.Application.Commands.EmployeesCommands;
using Cinema.Application.Handlers.EmployeesHandlers;

namespace TestHandlers.TestEmployeeHandlers
{
    public class CreateEmployeeHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateEmployeeHandler _handler;

        public CreateEmployeeHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateEmployeeHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidEmployee_CreatesEmployeeAndReturnsEmployeeDto()
        {
            var employeeForCreationDto = new EmployeeForCreationDto { Name = "Jane Doe", Role = "Manager" };
            var employee = new Employee { EmployeeId = Guid.NewGuid(), Name = employeeForCreationDto.Name, Role = employeeForCreationDto.Role };
            var command = new CreateEmployeeCommand(employeeForCreationDto);

            _mapperMock.Setup(m => m.Map<Employee>(employeeForCreationDto)).Returns(employee);
            _mapperMock.Setup(m => m.Map<EmployeeDto>(employee)).Returns(new EmployeeDto { EmployeeId = employee.EmployeeId, Name = employee.Name, Role = employee.Role });

            _repositoryMock.Setup(repo => repo.Employee.CreateEmployee(employee));
            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(employeeForCreationDto.Name, result.Name);
            Assert.Equal(employeeForCreationDto.Role, result.Role);
            Assert.Equal(employee.EmployeeId, result.EmployeeId);

            _repositoryMock.Verify(repo => repo.Employee.CreateEmployee(employee), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);

            _mapperMock.Verify(m => m.Map<Employee>(employeeForCreationDto), Times.Once);
            _mapperMock.Verify(m => m.Map<EmployeeDto>(employee), Times.Once);
        }
    }
}