using AutoMapper;
using Cinema.Application.Queries.WorkLogsQueries;
using Cinema.Application.Handlers.WorkLogsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestWorkLogHandlers
{
    public class GetWorkLogHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetWorkLogHandler _handler;

        public GetWorkLogHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetWorkLogHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_WorkLogNotFound_ReturnsWorkLogNotFoundResponse()
        {
            var workLogId = Guid.NewGuid();
            var command = new GetWorkLogQuery(workLogId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.WorkLog.GetWorkLogAsync(workLogId, false))
                .ReturnsAsync((WorkLog)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<WorkLogNotFoundResponse>(result);
            var response = result as WorkLogNotFoundResponse;
            Assert.Equal($"WorkLog with id: {workLogId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.WorkLog.GetWorkLogAsync(workLogId, false), Times.Once);
        }

        [Fact]
        public async Task Handle_WorkLogFound_ReturnsApiOkResponseWithWorkLogDto()
        {
            var workLogId = Guid.NewGuid();
            var workLog = new WorkLog
            {
                WorkLogId = workLogId,
                WorkExperience = 5,
                StartDate = DateOnly.FromDateTime(DateTime.Now),
                WorkHours = 8.0m
            };
            var workLogDto = new WorkLogDto
            {
                WorkLogId = workLogId,
                WorkExperience = workLog.WorkExperience,
                StartDate = workLog.StartDate,
                WorkHours = workLog.WorkHours
            };
            var command = new GetWorkLogQuery(workLogId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.WorkLog.GetWorkLogAsync(workLogId, false))
                .ReturnsAsync(workLog);

            _mapperMock.Setup(m => m.Map<WorkLogDto>(workLog)).Returns(workLogDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<WorkLogDto>>(result);
            var apiResponse = result as ApiOkResponse<WorkLogDto>;
            Assert.Equal(workLog.WorkLogId, apiResponse.Result.WorkLogId);
            Assert.Equal(workLog.WorkExperience, apiResponse.Result.WorkExperience);
            Assert.Equal(workLog.WorkHours, apiResponse.Result.WorkHours);
            Assert.Equal(workLog.StartDate, apiResponse.Result.StartDate);

            _repositoryMock.Verify(repo => repo.WorkLog.GetWorkLogAsync(workLogId, false), Times.Once);

            _mapperMock.Verify(m => m.Map<WorkLogDto>(workLog), Times.Once);
        }
    }
}