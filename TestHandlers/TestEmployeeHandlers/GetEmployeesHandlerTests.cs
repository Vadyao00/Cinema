using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;
using Cinema.Application.Handlers.EmployeesHandlers;
using Cinema.Application.Queries.EmployeesQueries;

namespace TestHandlers.TestEmployeeHandlers
{
    public class GetEmployeesHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetEmployeesHandler _handler;

        public GetEmployeesHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetEmployeesHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_EmployeesFound_ReturnsApiOkResponseWithEmployeeDtoAndMetaData()
        {
            var employeeParameters = new EmployeeParameters { PageNumber = 1, PageSize = 10 };
            var employees = new List<Employee>
            {
                new Employee { EmployeeId = Guid.NewGuid(), Name = "Alice Johnson", Role = "Developer" },
                new Employee { EmployeeId = Guid.NewGuid(), Name = "Bob Smith", Role = "Manager" }
            };

            var metaData = new MetaData
            {
                TotalCount = 2,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 1
            };

            var employeesDto = new List<EmployeeDto>
            {
                new EmployeeDto { EmployeeId = employees[0].EmployeeId, Name = employees[0].Name, Role = employees[0].Role },
                new EmployeeDto { EmployeeId = employees[1].EmployeeId, Name = employees[1].Name, Role = employees[1].Role }
            };

            var employeesWithMetaData = new PagedList<Employee>(employees, metaData.TotalCount, employeeParameters.PageNumber, employeeParameters.PageSize);

            var query = new GetEmployeesQuery(employeeParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Employee.GetEmployeesAsync(employeeParameters, false))
                .ReturnsAsync(employeesWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<EmployeeDto>>(employees)).Returns(employeesDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<EmployeeDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<EmployeeDto>, MetaData)>;
            Assert.Equal(2, apiResponse.Result.Item1.Count());
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.Employee.GetEmployeesAsync(employeeParameters, false), Times.Once);

            _mapperMock.Verify(m => m.Map<IEnumerable<EmployeeDto>>(employees), Times.Once);
        }

        [Fact]
        public async Task Handle_NoEmployeesFound_ReturnsApiOkResponseWithEmptyListAndMetaData()
        {
            var employeeParameters = new EmployeeParameters { PageNumber = 1, PageSize = 10 };
            var employees = new List<Employee>();

            var metaData = new MetaData
            {
                TotalCount = 0,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 0
            };

            var employeesDto = new List<EmployeeDto>();

            var employeesWithMetaData = new PagedList<Employee>(employees, metaData.TotalCount, employeeParameters.PageNumber, employeeParameters.PageSize);

            var query = new GetEmployeesQuery(employeeParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Employee.GetEmployeesAsync(employeeParameters, false))
                .ReturnsAsync(employeesWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<EmployeeDto>>(employees)).Returns(employeesDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<EmployeeDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<EmployeeDto>, MetaData)>;
            Assert.Empty(apiResponse.Result.Item1);
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.Employee.GetEmployeesAsync(employeeParameters, false), Times.Once);

            _mapperMock.Verify(m => m.Map<IEnumerable<EmployeeDto>>(employees), Times.Once);
        }
    }
}