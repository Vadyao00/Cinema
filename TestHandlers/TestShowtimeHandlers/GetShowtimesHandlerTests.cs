using AutoMapper;
using Cinema.Application.Queries.ShowtimesQueries;
using Cinema.Application.Handlers.ShowtimesHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestShowtimeHandlers
{
    public class GetShowtimesHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetShowtimesHandler _handler;

        public GetShowtimesHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetShowtimesHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsApiOkResponseWithShowtimeDtoAndMetaData()
        {
            var showtimeParameters = new ShowtimeParameters { PageNumber = 1, PageSize = 10 };
            var showtimes = new List<Showtime>
            {
                new Showtime { ShowtimeId = Guid.NewGuid(), StartTime = TimeOnly.FromDateTime(DateTime.Now), EndTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(2)) },
                new Showtime { ShowtimeId = Guid.NewGuid(), StartTime = TimeOnly.FromDateTime(DateTime.Now).AddHours(3), EndTime = TimeOnly.FromDateTime(DateTime.Now).AddHours(5) }
            };

            var metaData = new MetaData
            {
                TotalCount = 2,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 1
            };

            var showtimesDto = new List<ShowtimeDto>
            {
                new ShowtimeDto { ShowtimeId = showtimes[0].ShowtimeId, StartTime = showtimes[0].StartTime, EndTime = showtimes[0].EndTime },
                new ShowtimeDto { ShowtimeId = showtimes[1].ShowtimeId, StartTime = showtimes[1].StartTime, EndTime = showtimes[1].EndTime }
            };

            var showtimesWithMetaData = new PagedList<Showtime>(showtimes, metaData.TotalCount, showtimeParameters.PageNumber, showtimeParameters.PageSize);

            var command = new GetShowtimesQuery(showtimeParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Showtime.GetAllShowtimesAsync(showtimeParameters, false))
                .ReturnsAsync(showtimesWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<ShowtimeDto>>(showtimes)).Returns(showtimesDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<ShowtimeDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<ShowtimeDto>, MetaData)>;
            Assert.Equal(2, apiResponse.Result.Item1.Count());
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.Showtime.GetAllShowtimesAsync(showtimeParameters, false), Times.Once);

            _mapperMock.Verify(m => m.Map<IEnumerable<ShowtimeDto>>(showtimes), Times.Once);
        }

        [Fact]
        public async Task Handle_NoShowtimesFound_ReturnsApiOkResponseWithEmptyListAndMetaData()
        {
            var showtimeParameters = new ShowtimeParameters { PageNumber = 1, PageSize = 10 };
            var showtimes = new List<Showtime>();

            var metaData = new MetaData
            {
                TotalCount = 0,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 0
            };

            var showtimesDto = new List<ShowtimeDto>();

            var showtimesWithMetaData = new PagedList<Showtime>(showtimes, metaData.TotalCount, showtimeParameters.PageNumber, showtimeParameters.PageSize);

            var command = new GetShowtimesQuery(showtimeParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Showtime.GetAllShowtimesAsync(showtimeParameters, false))
                .ReturnsAsync(showtimesWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<ShowtimeDto>>(showtimes)).Returns(showtimesDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<ShowtimeDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<ShowtimeDto>, MetaData)>;
            Assert.Empty(apiResponse.Result.Item1);
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.Showtime.GetAllShowtimesAsync(showtimeParameters, false), Times.Once);

            _mapperMock.Verify(m => m.Map<IEnumerable<ShowtimeDto>>(showtimes), Times.Once);
        }
    }
}