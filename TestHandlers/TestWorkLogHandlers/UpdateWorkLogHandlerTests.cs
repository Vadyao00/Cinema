using AutoMapper;
using Cinema.Application.Commands.WorkLogCommands;
using Cinema.Application.Handlers.WorkLogsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestWorkLogHandlers
{
    public class UpdateWorkLogHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateWorkLogHandler _handler;

        public UpdateWorkLogHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateWorkLogHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WorkLogNotFound_ReturnsWorkLogNotFoundResponse()
        {
            var workLogId = Guid.NewGuid();
            var workLogForUpdateDto = new WorkLogForUpdateDto
            {
                WorkExperience = 10,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                WorkHours = 8.0m
            };
            var command = new UpdateWorkLogCommand(workLogId, workLogForUpdateDto, WrkTrackChanges: true, EmpTrackChanges: false);

            _repositoryMock.Setup(repo => repo.WorkLog.GetWorkLogAsync(workLogId, true))
                .ReturnsAsync((WorkLog)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<WorkLogNotFoundResponse>(result);
            var response = result as WorkLogNotFoundResponse;
            Assert.Equal($"WorkLog with id: {workLogId} is not found in db.", response.Message);
        }
    }
}