using AutoMapper;
using Cinema.Application.Commands.EmployeesCommands;
using Cinema.Application.Handlers.EmployeesHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestEmployeeHandlers
{
    public class UpdateEmployeeHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateEmployeeHandler _handler;

        public UpdateEmployeeHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateEmployeeHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_EmployeeNotFound_ReturnsEmployeeNotFoundResponse()
        {
            var employeeId = Guid.NewGuid();
            var employeeForUpdateDto = new EmployeeForUpdateDto { Name = "Updated Employee" };
            var command = new UpdateEmployeeCommand(employeeId, employeeForUpdateDto, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Employee.GetEmployeeAsync(employeeId, false))
                .ReturnsAsync((Employee)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<EmployeeNotFoundResponse>(result);
            var response = result as EmployeeNotFoundResponse;
            Assert.Equal($"Employee with id: {employeeId} is not found in db.", response.Message);
        }
    }
}