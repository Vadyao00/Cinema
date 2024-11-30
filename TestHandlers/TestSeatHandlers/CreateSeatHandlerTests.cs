using AutoMapper;
using Cinema.Application.Commands.SeatsCommands;
using Cinema.Application.Handlers.SeatsHandlers;
using Cinema.Controllers.Extensions;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestSeatHandlers
{
    public class CreateSeatHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateSeatHandler _handler;

        public CreateSeatHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateSeatHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_InvalidEventId_ReturnsEventNotFoundResponse()
        {
            var seatForCreationDto = new SeatForCreationDto { SeatNumber = 10};
            var seat = new Seat { SeatId = Guid.NewGuid(), SeatNumber = seatForCreationDto.SeatNumber };
            var command = new CreateSeatCommand(ShowtimeId: null, EventId: Guid.NewGuid(), seatForCreationDto, TrackChanges: false);

            _mapperMock.Setup(m => m.Map<Seat>(seatForCreationDto)).Returns(seat);
            _repositoryMock.Setup(repo => repo.Seat.CreateSeat(command.EventId, command.ShowtimeId, seat));
            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
            _repositoryMock.Setup(repo => repo.Event.GetEventAsync(command.EventId.Value, false)).ReturnsAsync((Event)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<EventNotFoundResponse>(result);
            var response = result as EventNotFoundResponse;
            Assert.Equal($"Event with id: {command.EventId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Seat.CreateSeat(command.EventId, command.ShowtimeId, seat), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidShowtimeId_ReturnsShowtimeNotFoundResponse()
        {
            var seatForCreationDto = new SeatForCreationDto { SeatNumber = 10};
            var seat = new Seat { SeatId = Guid.NewGuid(), SeatNumber = seatForCreationDto.SeatNumber };
            var command = new CreateSeatCommand(ShowtimeId: Guid.NewGuid(), EventId: null, seatForCreationDto, TrackChanges: false);

            _mapperMock.Setup(m => m.Map<Seat>(seatForCreationDto)).Returns(seat);
            _repositoryMock.Setup(repo => repo.Seat.CreateSeat(command.EventId, command.ShowtimeId, seat));
            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);
            _repositoryMock.Setup(repo => repo.Showtime.GetShowtimeAsync(command.ShowtimeId.Value, false)).ReturnsAsync((Showtime)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ShowtimeNotFoundResponse>(result);
            var response = result as ShowtimeNotFoundResponse;
            Assert.Equal($"Showtime with id: {command.ShowtimeId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Seat.CreateSeat(command.EventId, command.ShowtimeId, seat), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}