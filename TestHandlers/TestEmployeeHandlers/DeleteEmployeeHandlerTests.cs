using Cinema.Application.Commands.EmployeesCommands;
using Cinema.Application.Handlers.EmployeesHandlers;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestEmployeeHandlers
{
    public class DeleteEmployeeHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly DeleteEmployeeHandler _handler;

        public DeleteEmployeeHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _handler = new DeleteEmployeeHandler(_repositoryMock.Object, null);
        }

        [Fact]
        public async Task Handle_EmployeeNotFound_ReturnsEmployeeNotFoundResponse()
        {
            var employeeId = Guid.NewGuid();
            var command = new DeleteEmployeeCommand(employeeId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Employee.GetEmployeeAsync(employeeId, false))
                .ReturnsAsync((Employee)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<EmployeeNotFoundResponse>(result);
            var response = result as EmployeeNotFoundResponse;
            Assert.Equal($"Employee with id: {employeeId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Employee.DeleteEmployee(It.IsAny<Employee>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_EmployeeFound_DeletesEmployeeAndReturnsApiOkResponse()
        {
            var employeeId = Guid.NewGuid();
            var employee = new Employee { EmployeeId = employeeId, Name = "Jane Doe", Role = "Manager" };
            var command = new DeleteEmployeeCommand(employeeId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Employee.GetEmployeeAsync(employeeId, false))
                .ReturnsAsync(employee);

            _repositoryMock.Setup(repo => repo.Employee.DeleteEmployee(employee));
            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<Employee>>(result);
            var apiResponse = result as ApiOkResponse<Employee>;
            Assert.Equal(employee.EmployeeId, apiResponse.Result.EmployeeId);
            Assert.Equal(employee.Name, apiResponse.Result.Name);
            Assert.Equal(employee.Role, apiResponse.Result.Role);

            _repositoryMock.Verify(repo => repo.Employee.DeleteEmployee(employee), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}