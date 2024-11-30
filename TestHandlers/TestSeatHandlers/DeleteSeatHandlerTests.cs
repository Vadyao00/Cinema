using Cinema.Application.Commands.SeatsCommands;
using Cinema.Application.Handlers.SeatsHandlers;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestSeatHandlers
{
    public class DeleteSeatHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly DeleteSeatHandler _handler;

        public DeleteSeatHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _handler = new DeleteSeatHandler(_repositoryMock.Object, null);
        }

        [Fact]
        public async Task Handle_SeatNotFound_ReturnsSeatNotFoundResponse()
        {
            var seatId = Guid.NewGuid();
            var command = new DeleteSeatCommand(seatId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Seat.GetSeatAsync(seatId, false))
                .ReturnsAsync((Seat)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<SeatNotFoundResponse>(result);
            var response = result as SeatNotFoundResponse;
            Assert.Equal($"Seat with id: {seatId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Seat.DeleteSeat(It.IsAny<Seat>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_SeatFound_DeletesSeatAndReturnsApiOkResponse()
        {
            var seatId = Guid.NewGuid();
            var seat = new Seat { SeatId = seatId, SeatNumber = 1 };
            var command = new DeleteSeatCommand(seatId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Seat.GetSeatAsync(seatId, false))
                .ReturnsAsync(seat);

            _repositoryMock.Setup(repo => repo.Seat.DeleteSeat(seat));
            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<Seat>>(result);
            var apiResponse = result as ApiOkResponse<Seat>;
            Assert.Equal(seat.SeatId, apiResponse.Result.SeatId);
            Assert.Equal(seat.SeatNumber, apiResponse.Result.SeatNumber);

            _repositoryMock.Verify(repo => repo.Seat.DeleteSeat(seat), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}