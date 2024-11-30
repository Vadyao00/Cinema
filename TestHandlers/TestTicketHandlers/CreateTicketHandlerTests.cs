using AutoMapper;
using Cinema.Application.Commands.TicketsCommands;
using Cinema.Application.Handlers.TicketsHandlers;
using Cinema.Controllers.Extensions;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestTicketHandlers
{
    public class CreateTicketHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateTicketHandler _handler;

        public CreateTicketHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateTicketHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidTicket_CreatesTicketAndReturnsTicketDto()
        {
            var purchaseDate = DateOnly.FromDateTime(DateTime.Now);
            var ticketForCreationDto = new TicketForCreationDto { PurchaseDate = purchaseDate };
            var seat = new Seat { SeatId = Guid.NewGuid(), SeatNumber = 13};
            var ticket = new Ticket { TicketId = Guid.NewGuid(), PurchaseDate = ticketForCreationDto.PurchaseDate, Seat = seat };
            var command = new CreateTicketCommand(seat.SeatId, ticketForCreationDto, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Seat.GetSeatAsync(seat.SeatId, false)).ReturnsAsync(seat);
            _mapperMock.Setup(m => m.Map<Ticket>(ticketForCreationDto)).Returns(ticket);
            _mapperMock.Setup(m => m.Map<TicketDto>(ticket)).Returns(new TicketDto { TicketId = ticket.TicketId, SeatNumber = ticket.Seat.SeatNumber, PurchaseDate = ticket.PurchaseDate });

            _repositoryMock.Setup(repo => repo.Ticket.CreateTicketForSeat(seat.SeatId, ticket));
            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(ticket.PurchaseDate, result.GetResult<TicketDto>().PurchaseDate);
            Assert.Equal(ticket.TicketId, result.GetResult<TicketDto>().TicketId);

            _repositoryMock.Verify(repo => repo.Ticket.CreateTicketForSeat(seat.SeatId, ticket), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<Ticket>(ticketForCreationDto), Times.Once);
            _mapperMock.Verify(m => m.Map<TicketDto>(ticket), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidSeatId_ReturnsSeatNotFoundResponse()
        {
            var purchaseDate = DateOnly.FromDateTime(DateTime.Now);
            var ticketForCreationDto = new TicketForCreationDto { PurchaseDate = purchaseDate };
            var command = new CreateTicketCommand(Guid.NewGuid(), ticketForCreationDto, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Seat.GetSeatAsync(command.SeatId, false)).ReturnsAsync((Seat)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<SeatNotFoundResponse>(result);
            var response = result as SeatNotFoundResponse;
            Assert.Equal($"Seat with id: {command.SeatId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Seat.GetSeatAsync(command.SeatId, false), Times.Once);
        }
    }
}