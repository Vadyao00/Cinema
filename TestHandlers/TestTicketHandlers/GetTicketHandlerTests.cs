using AutoMapper;
using Cinema.Application.Queries.TicketsQueries;
using Cinema.Application.Handlers.TicketsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestTicketHandlers
{
    public class GetTicketHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetTicketHandler _handler;

        public GetTicketHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetTicketHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_TicketNotFound_ReturnsTicketNotFoundResponse()
        {
            var ticketId = Guid.NewGuid();
            var command = new GetTicketQuery(ticketId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Ticket.GetTicketAsync(ticketId, false))
                .ReturnsAsync((Ticket)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<TicketNotFoundResponse>(result);
            var response = result as TicketNotFoundResponse;
            Assert.Equal($"Ticket with id: {ticketId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Ticket.GetTicketAsync(ticketId, false), Times.Once);
        }

        [Fact]
        public async Task Handle_TicketFound_ReturnsApiOkResponseWithTicketDto()
        {
            var ticketId = Guid.NewGuid();
            var ticket = new Ticket { TicketId = ticketId, PurchaseDate = DateOnly.FromDateTime(DateTime.Now) };
            var ticketDto = new TicketDto { TicketId = ticketId, PurchaseDate = ticket.PurchaseDate };
            var command = new GetTicketQuery(ticketId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Ticket.GetTicketAsync(ticketId, false))
                .ReturnsAsync(ticket);

            _mapperMock.Setup(m => m.Map<TicketDto>(ticket)).Returns(ticketDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<TicketDto>>(result);
            var apiResponse = result as ApiOkResponse<TicketDto>;
            Assert.Equal(ticket.TicketId, apiResponse.Result.TicketId);
            Assert.Equal(ticket.PurchaseDate, apiResponse.Result.PurchaseDate);

            _repositoryMock.Verify(repo => repo.Ticket.GetTicketAsync(ticketId, false), Times.Once);
            _mapperMock.Verify(m => m.Map<TicketDto>(ticket), Times.Once);
        }
    }
}