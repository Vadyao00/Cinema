using AutoMapper;
using Cinema.Application.Commands.ShowtimesCommands;
using Cinema.Application.Handlers.ShowtimesHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestShowtimeHandlers
{
    public class UpdateShowtimeHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateShowtimeHandler _handler;

        public UpdateShowtimeHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateShowtimeHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShowtimeNotFound_ReturnsShowtimeNotFoundResponse()
        {
            var showtimeId = Guid.NewGuid();
            var showtimeForUpdateDto = new ShowtimeForUpdateDto
            {
                StartTime = TimeOnly.FromDateTime(DateTime.Now).AddHours(1),
                EndTime = TimeOnly.FromDateTime(DateTime.Now).AddHours(3)
            };
            var command = new UpdateShowtimeCommand(showtimeId, showtimeForUpdateDto, MovTrackChanges: false, ShwTrackChanges: true);

            _repositoryMock.Setup(repo => repo.Showtime.GetShowtimeAsync(showtimeId, false))
                .ReturnsAsync((Showtime)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ShowtimeNotFoundResponse>(result);
            var response = result as ShowtimeNotFoundResponse;
            Assert.Equal($"Showtime with id: {showtimeId} is not found in db.", response.Message);
        }
    }
}