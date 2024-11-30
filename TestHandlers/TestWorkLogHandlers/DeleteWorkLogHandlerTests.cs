using Cinema.Application.Commands.WorkLogCommands;
using Cinema.Application.Handlers.WorkLogsHandlers;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestWorkLogHandlers
{
    public class DeleteWorkLogHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly DeleteWorkLogHandler _handler;

        public DeleteWorkLogHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _handler = new DeleteWorkLogHandler(_repositoryMock.Object, null);
        }

        [Fact]
        public async Task Handle_WorkLogNotFound_ReturnsWorkLogNotFoundResponse()
        {
            var workLogId = Guid.NewGuid();
            var command = new DeleteWorkLogCommand(workLogId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.WorkLog.GetWorkLogAsync(workLogId, false))
                .ReturnsAsync((WorkLog)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<WorkLogNotFoundResponse>(result);
            var response = result as WorkLogNotFoundResponse;
            Assert.Equal($"WorkLog with id: {workLogId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.WorkLog.DeleteWorkLog(It.IsAny<WorkLog>()), Times.Never);

            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_WorkLogFound_DeletesWorkLogAndReturnsApiOkResponse()
        {
            var workLogId = Guid.NewGuid();
            var workLog = new WorkLog
            {
                WorkLogId = workLogId,
                WorkExperience = 5,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                WorkHours = 8.0m
            };
            var command = new DeleteWorkLogCommand(workLogId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.WorkLog.GetWorkLogAsync(workLogId, false))
                .ReturnsAsync(workLog);

            _repositoryMock.Setup(repo => repo.WorkLog.DeleteWorkLog(workLog));

            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<WorkLog>>(result);
            var apiResponse = result as ApiOkResponse<WorkLog>;
            Assert.Equal(workLog.WorkLogId, apiResponse.Result.WorkLogId);
            Assert.Equal(workLog.WorkExperience, apiResponse.Result.WorkExperience);
            Assert.Equal(workLog.WorkHours, apiResponse.Result.WorkHours);

            _repositoryMock.Verify(repo => repo.WorkLog.DeleteWorkLog(workLog), Times.Once);

            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}