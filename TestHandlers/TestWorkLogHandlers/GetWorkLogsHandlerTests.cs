using AutoMapper;
using Cinema.Application.Queries.WorkLogsQueries;
using Cinema.Application.Handlers.WorkLogsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestWorkLogHandlers
{
    public class GetWorkLogsHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetWorkLogsHandler _handler;

        public GetWorkLogsHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetWorkLogsHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsApiOkResponseWithWorkLogDtoAndMetaData()
        {
            var workLogParameters = new WorkLogParameters { PageNumber = 1, PageSize = 10 };
            var workLogs = new List<WorkLog>
            {
                new WorkLog { WorkLogId = Guid.NewGuid(), WorkExperience = 5, StartDate = DateOnly.FromDateTime(DateTime.Now), WorkHours = 8.0m },
                new WorkLog { WorkLogId = Guid.NewGuid(), WorkExperience = 3, StartDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1), WorkHours = 7.0m }
            };

            var metaData = new MetaData
            {
                TotalCount = 2,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 1
            };

            var workLogsDto = new List<WorkLogDto>
            {
                new WorkLogDto { WorkLogId = workLogs[0].WorkLogId, WorkExperience = workLogs[0].WorkExperience, StartDate = workLogs[0].StartDate, WorkHours = workLogs[0].WorkHours },
                new WorkLogDto { WorkLogId = workLogs[1].WorkLogId, WorkExperience = workLogs[1].WorkExperience, StartDate = workLogs[1].StartDate, WorkHours = workLogs[1].WorkHours }
            };

            var workLogsWithMetaData = new PagedList<WorkLog>(workLogs, metaData.TotalCount, workLogParameters.PageNumber, workLogParameters.PageSize);

            var command = new GetWorkLogsQuery(workLogParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.WorkLog.GetAllWorkLogsAsync(workLogParameters, false))
                .ReturnsAsync(workLogsWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<WorkLogDto>>(workLogs)).Returns(workLogsDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<WorkLogDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<WorkLogDto>, MetaData)>;
            Assert.Equal(2, apiResponse.Result.Item1.Count());
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.WorkLog.GetAllWorkLogsAsync(workLogParameters, false), Times.Once);

            _mapperMock.Verify(m => m.Map<IEnumerable<WorkLogDto>>(workLogs), Times.Once);
        }

        [Fact]
        public async Task Handle_NoWorkLogsFound_ReturnsApiOkResponseWithEmptyListAndMetaData()
        {
            var workLogParameters = new WorkLogParameters { PageNumber = 1, PageSize = 10 };
            var workLogs = new List<WorkLog>();

            var metaData = new MetaData
            {
                TotalCount = 0,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 0
            };

            var workLogsDto = new List<WorkLogDto>();

            var workLogsWithMetaData = new PagedList<WorkLog>(workLogs, metaData.TotalCount, workLogParameters.PageNumber, workLogParameters.PageSize);

            var command = new GetWorkLogsQuery(workLogParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.WorkLog.GetAllWorkLogsAsync(workLogParameters, false))
                .ReturnsAsync(workLogsWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<WorkLogDto>>(workLogs)).Returns(workLogsDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<WorkLogDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<WorkLogDto>, MetaData)>;
            Assert.Empty(apiResponse.Result.Item1);
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.WorkLog.GetAllWorkLogsAsync(workLogParameters, false), Times.Once);

            _mapperMock.Verify(m => m.Map<IEnumerable<WorkLogDto>>(workLogs), Times.Once);
        }
    }
}