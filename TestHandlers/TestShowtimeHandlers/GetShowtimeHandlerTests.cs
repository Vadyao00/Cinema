using AutoMapper;
using Cinema.Application.Queries.ShowtimesQueries;
using Cinema.Application.Handlers.ShowtimesHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestShowtimeHandlers
{
    public class GetShowtimeHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetShowtimeHandler _handler;

        public GetShowtimeHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetShowtimeHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShowtimeNotFound_ReturnsShowtimeNotFoundResponse()
        {
            var showtimeId = Guid.NewGuid();
            var command = new GetShowtimeQuery(showtimeId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Showtime.GetShowtimeAsync(showtimeId, false))
                .ReturnsAsync((Showtime)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ShowtimeNotFoundResponse>(result);
            var response = result as ShowtimeNotFoundResponse;
            Assert.Equal($"Showtime with id: {showtimeId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Showtime.GetShowtimeAsync(showtimeId, false), Times.Once);
        }

        [Fact]
        public async Task Handle_ShowtimeFound_ReturnsApiOkResponseWithShowtimeDto()
        {
            var showtimeId = Guid.NewGuid();
            var showtime = new Showtime { ShowtimeId = showtimeId, StartTime = TimeOnly.FromDateTime(DateTime.Now), EndTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(2)) };
            var showtimeDto = new ShowtimeDto { ShowtimeId = showtimeId, StartTime = showtime.StartTime, EndTime = showtime.EndTime };
            var command = new GetShowtimeQuery(showtimeId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Showtime.GetShowtimeAsync(showtimeId, false))
                .ReturnsAsync(showtime);

            _mapperMock.Setup(m => m.Map<ShowtimeDto>(showtime)).Returns(showtimeDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<ShowtimeDto>>(result);
            var apiResponse = result as ApiOkResponse<ShowtimeDto>;
            Assert.Equal(showtime.ShowtimeId, apiResponse.Result.ShowtimeId);
            Assert.Equal(showtime.StartTime, apiResponse.Result.StartTime);
            Assert.Equal(showtime.EndTime, apiResponse.Result.EndTime);

            _repositoryMock.Verify(repo => repo.Showtime.GetShowtimeAsync(showtimeId, false), Times.Once);

            _mapperMock.Verify(m => m.Map<ShowtimeDto>(showtime), Times.Once);
        }
    }
}