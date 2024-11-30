using AutoMapper;
using Cinema.Application.Commands.WorkLogCommands;
using Cinema.Application.Handlers.WorkLogsHandlers;
using Cinema.Controllers.Extensions;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestWorkLogHandlers
{
    public class CreateWorkLogHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateWorkLogHandler _handler;

        public CreateWorkLogHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateWorkLogHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidWorkLog_CreatesWorkLogAndReturnsWorkLogDto()
        {
            var workLogForCreationDto = new WorkLogForCreationDto
            {
                WorkExperience = 5,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                WorkHours = 8.0m
            };
            var employee = new Employee { EmployeeId = Guid.NewGuid(), Name = "John Doe" };
            var workLog = new WorkLog
            {
                WorkLogId = Guid.NewGuid(),
                WorkExperience = workLogForCreationDto.WorkExperience,
                StartDate = workLogForCreationDto.StartDate,
                WorkHours = workLogForCreationDto.WorkHours
            };
            var command = new CreateWorkLogCommand(employee.EmployeeId, workLogForCreationDto, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Employee.GetEmployeeAsync(employee.EmployeeId, false)).ReturnsAsync(employee);
            _mapperMock.Setup(m => m.Map<WorkLog>(workLogForCreationDto)).Returns(workLog);
            _mapperMock.Setup(m => m.Map<WorkLogDto>(workLog)).Returns(new WorkLogDto { WorkLogId = workLog.WorkLogId, WorkExperience = workLog.WorkExperience, StartDate = workLog.StartDate, WorkHours = workLog.WorkHours });

            _repositoryMock.Setup(repo => repo.WorkLog.CreateWorkLogForEmployee(employee.EmployeeId, workLog));
            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(workLogForCreationDto.WorkExperience, result.GetResult<WorkLogDto>().WorkExperience);
            Assert.Equal(workLogForCreationDto.StartDate, result.GetResult<WorkLogDto>().StartDate);
            Assert.Equal(workLogForCreationDto.WorkHours, result.GetResult<WorkLogDto>().WorkHours);
            Assert.Equal(workLog.WorkLogId, result.GetResult<WorkLogDto>().WorkLogId);

            _repositoryMock.Verify(repo => repo.WorkLog.CreateWorkLogForEmployee(employee.EmployeeId, workLog), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<WorkLog>(workLogForCreationDto), Times.Once);
            _mapperMock.Verify(m => m.Map<WorkLogDto>(workLog), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidEmployeeId_ReturnsEmployeeNotFoundResponse()
        {
            var workLogForCreationDto = new WorkLogForCreationDto
            {
                WorkExperience = 5,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                WorkHours = 8.0m
            };
            var command = new CreateWorkLogCommand(Guid.NewGuid(), workLogForCreationDto, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Employee.GetEmployeeAsync(command.EmployeeId, false)).ReturnsAsync((Employee)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<EmployeeNotFoundResponse>(result);
            var response = result as EmployeeNotFoundResponse;
            Assert.Equal($"Employee with id: {command.EmployeeId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Employee.GetEmployeeAsync(command.EmployeeId, false), Times.Once);
        }
    }
}