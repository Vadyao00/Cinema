using AutoMapper;
using Cinema.Application.Queries.SeatsQueries;
using Cinema.Application.Handlers.SeatsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestSeatHandlers
{
    public class GetSeatHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetSeatHandler _handler;

        public GetSeatHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetSeatHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_SeatNotFound_ReturnsSeatNotFoundResponse()
        {
            var seatId = Guid.NewGuid();
            var command = new GetSeatQuery(seatId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Seat.GetSeatAsync(seatId, false))
                .ReturnsAsync((Seat)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<SeatNotFoundResponse>(result);
            var response = result as SeatNotFoundResponse;
            Assert.Equal($"Seat with id: {seatId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Seat.GetSeatAsync(seatId, false), Times.Once);
        }

        [Fact]
        public async Task Handle_SeatFound_ReturnsApiOkResponseWithSeatDto()
        {
            var seatId = Guid.NewGuid();
            var seat = new Seat { SeatId = seatId, SeatNumber = 1 };
            var seatDto = new SeatDto { SeatId = seatId, SeatNumber = seat.SeatNumber };
            var command = new GetSeatQuery(seatId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Seat.GetSeatAsync(seatId, false))
                .ReturnsAsync(seat);

            _mapperMock.Setup(m => m.Map<SeatDto>(seat)).Returns(seatDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<SeatDto>>(result);
            var apiResponse = result as ApiOkResponse<SeatDto>;
            Assert.Equal(seat.SeatId, apiResponse.Result.SeatId);
            Assert.Equal(seat.SeatNumber, apiResponse.Result.SeatNumber);

            _repositoryMock.Verify(repo => repo.Seat.GetSeatAsync(seatId, false), Times.Once);

            _mapperMock.Verify(m => m.Map<SeatDto>(seat), Times.Once);
        }
    }
}