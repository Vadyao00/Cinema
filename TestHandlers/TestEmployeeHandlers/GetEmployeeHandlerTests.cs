using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;
using Cinema.Application.Handlers.EmployeesHandlers;
using Cinema.Application.Queries.EmployeesQueries;

namespace TestHandlers.TestEmployeeHandlers
{
    public class GetEmployeeHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetEmployeeHandler _handler;

        public GetEmployeeHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetEmployeeHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_EmployeeNotFound_ReturnsEmployeeNotFoundResponse()
        {
            var employeeId = Guid.NewGuid();
            var query = new GetEmployeeQuery(employeeId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Employee.GetEmployeeAsync(employeeId, false))
                .ReturnsAsync((Employee)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<EmployeeNotFoundResponse>(result);
            var response = result as EmployeeNotFoundResponse;
            Assert.Equal($"Employee with id: {employeeId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Employee.GetEmployeeAsync(employeeId, false), Times.Once);
        }

        [Fact]
        public async Task Handle_EmployeeFound_ReturnsApiOkResponseWithEmployeeDto()
        {
            var employeeId = Guid.NewGuid();
            var employee = new Employee { EmployeeId = employeeId, Name = "Jane Doe", Role = "Manager" };
            var employeeDto = new EmployeeDto { EmployeeId = employeeId, Name = "Jane Doe", Role = "Manager" };
            var query = new GetEmployeeQuery(employeeId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Employee.GetEmployeeAsync(employeeId, false))
                .ReturnsAsync(employee);

            _mapperMock.Setup(m => m.Map<EmployeeDto>(employee)).Returns(employeeDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<ApiOkResponse<EmployeeDto>>(result);
            var apiResponse = result as ApiOkResponse<EmployeeDto>;
            Assert.Equal(employeeDto.EmployeeId, apiResponse.Result.EmployeeId);
            Assert.Equal(employeeDto.Name, apiResponse.Result.Name);
            Assert.Equal(employeeDto.Role, apiResponse.Result.Role);

            _repositoryMock.Verify(repo => repo.Employee.GetEmployeeAsync(employeeId, false), Times.Once);
            _mapperMock.Verify(m => m.Map<EmployeeDto>(employee), Times.Once);
        }
    }
}