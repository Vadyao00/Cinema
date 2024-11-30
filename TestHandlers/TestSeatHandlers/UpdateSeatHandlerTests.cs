using AutoMapper;
using Cinema.Application.Commands.SeatsCommands;
using Cinema.Application.Handlers.SeatsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestSeatHandlers
{
    public class UpdateSeatHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateSeatHandler _handler;

        public UpdateSeatHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateSeatHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_SeatNotFound_ReturnsSeatNotFoundResponse()
        {
            var seatId = Guid.NewGuid();
            var seatForUpdateDto = new SeatForUpdateDto { SeatNumber = 5 };
            var command = new UpdateSeatCommand(seatId, seatForUpdateDto, SeatTrackChanges: false);

            _repositoryMock.Setup(repo => repo.Seat.GetSeatAsync(seatId, false))
                .ReturnsAsync((Seat)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<SeatNotFoundResponse>(result);
            var response = result as SeatNotFoundResponse;
            Assert.Equal($"Seat with id: {seatId} is not found in db.", response.Message);
        }
    }
}