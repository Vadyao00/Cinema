using AutoMapper;
using Cinema.Application.Commands.TicketsCommands;
using Cinema.Application.Handlers.TicketsHandlers;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestTicketHandlers
{
    public class DeleteTicketHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeleteTicketHandler _handler;

        public DeleteTicketHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new DeleteTicketHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_TicketNotFound_ReturnsTicketNotFoundResponse()
        {
            var ticketId = Guid.NewGuid();
            var command = new DeleteTicketCommand(ticketId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Ticket.GetTicketAsync(ticketId, false))
                .ReturnsAsync((Ticket)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<TicketNotFoundResponse>(result);
            var response = result as TicketNotFoundResponse;
            Assert.Equal($"Ticket with id: {ticketId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Ticket.DeleteTicket(It.IsAny<Ticket>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_TicketFound_DeletesTicketAndReturnsApiOkResponse()
        {
            var ticketId = Guid.NewGuid();
            var ticket = new Ticket { TicketId = ticketId, PurchaseDate = DateOnly.FromDateTime(DateTime.Now) };
            var command = new DeleteTicketCommand(ticketId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Ticket.GetTicketAsync(ticketId, false))
                .ReturnsAsync(ticket);

            _repositoryMock.Setup(repo => repo.Ticket.DeleteTicket(ticket));

            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<Ticket>>(result);
            var apiResponse = result as ApiOkResponse<Ticket>;
            Assert.Equal(ticket.TicketId, apiResponse.Result.TicketId);
            Assert.Equal(ticket.PurchaseDate, apiResponse.Result.PurchaseDate);

            _repositoryMock.Verify(repo => repo.Ticket.DeleteTicket(ticket), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}