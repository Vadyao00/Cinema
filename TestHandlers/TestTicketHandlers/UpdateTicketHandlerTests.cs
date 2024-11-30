using AutoMapper;
using Cinema.Application.Commands.TicketsCommands;
using Cinema.Application.Handlers.TicketsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestTicketHandlers
{
    public class UpdateTicketHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateTicketHandler _handler;

        public UpdateTicketHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateTicketHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_TicketNotFound_ReturnsTicketNotFoundResponse()
        {
            var ticketId = Guid.NewGuid();
            var ticketForUpdateDto = new TicketForUpdateDto
            {
                PurchaseDate = DateOnly.FromDateTime(DateTime.Now)
            };
            var command = new UpdateTicketCommand(ticketId, ticketForUpdateDto, TickTrackChanges: true, SeatTrackChanges: false);

            _repositoryMock.Setup(repo => repo.Ticket.GetTicketAsync(ticketId, true))
                .ReturnsAsync((Ticket)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<TicketNotFoundResponse>(result);
            var response = result as TicketNotFoundResponse;
            Assert.Equal($"Ticket with id: {ticketId} is not found in db.", response.Message);
        }
    }
}