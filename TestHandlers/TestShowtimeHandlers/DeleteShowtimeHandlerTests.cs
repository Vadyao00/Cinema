using Cinema.Application.Commands.ShowtimesCommands;
using Cinema.Application.Handlers.ShowtimesHandlers;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestShowtimeHandlers
{
    public class DeleteShowtimeHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly DeleteShowtimeHandler _handler;

        public DeleteShowtimeHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _handler = new DeleteShowtimeHandler(_repositoryMock.Object, null);
        }

        [Fact]
        public async Task Handle_ShowtimeNotFound_ReturnsShowtimeNotFoundResponse()
        {
            var showtimeId = Guid.NewGuid();
            var command = new DeleteShowtimeCommand(showtimeId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Showtime.GetShowtimeAsync(showtimeId, false))
                .ReturnsAsync((Showtime)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ShowtimeNotFoundResponse>(result);
            var response = result as ShowtimeNotFoundResponse;
            Assert.Equal($"Showtime with id: {showtimeId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Showtime.DeleteShowtimeForMovie(It.IsAny<Showtime>()), Times.Never);

            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShowtimeFound_DeletesShowtimeAndReturnsApiOkResponse()
        {
            var showtimeId = Guid.NewGuid();
            var showtime = new Showtime { ShowtimeId = showtimeId, StartTime = TimeOnly.FromDateTime(DateTime.Now), EndTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(2)) };
            var command = new DeleteShowtimeCommand(showtimeId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Showtime.GetShowtimeAsync(showtimeId, false))
                .ReturnsAsync(showtime);

            _repositoryMock.Setup(repo => repo.Showtime.DeleteShowtimeForMovie(showtime));

            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<Showtime>>(result);
            var apiResponse = result as ApiOkResponse<Showtime>;
            Assert.Equal(showtime.ShowtimeId, apiResponse.Result.ShowtimeId);
            Assert.Equal(showtime.StartTime, apiResponse.Result.StartTime);
            Assert.Equal(showtime.EndTime, apiResponse.Result.EndTime);

            _repositoryMock.Verify(repo => repo.Showtime.DeleteShowtimeForMovie(showtime), Times.Once);

            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}