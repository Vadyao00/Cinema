using AutoMapper;
using Cinema.Application.Queries.TicketsQueries;
using Cinema.Application.Handlers.TicketsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestTicketHandlers
{
    public class GetTicketsHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetTicketsHandler _handler;

        public GetTicketsHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetTicketsHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsApiOkResponseWithTicketDtoAndMetaData()
        {
            var ticketParameters = new TicketParameters { PageNumber = 1, PageSize = 10 };
            var tickets = new List<Ticket>
            {
                new Ticket { TicketId = Guid.NewGuid(), PurchaseDate = DateOnly.FromDateTime(DateTime.Now) },
                new Ticket { TicketId = Guid.NewGuid(), PurchaseDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) }
            };

            var metaData = new MetaData
            {
                TotalCount = 2,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 1
            };

            var ticketsDto = new List<TicketDto>
            {
                new TicketDto { TicketId = tickets[0].TicketId, PurchaseDate = tickets[0].PurchaseDate },
                new TicketDto { TicketId = tickets[1].TicketId, PurchaseDate = tickets[1].PurchaseDate }
            };

            var ticketsWithMetaData = new PagedList<Ticket>(tickets, metaData.TotalCount, ticketParameters.PageNumber, ticketParameters.PageSize);

            var command = new GetTicketsQuery(ticketParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Ticket.GetAllTicketsAsync(ticketParameters, false))
                .ReturnsAsync(ticketsWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<TicketDto>>(tickets)).Returns(ticketsDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<TicketDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<TicketDto>, MetaData)>;
            Assert.Equal(2, apiResponse.Result.Item1.Count());
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.Ticket.GetAllTicketsAsync(ticketParameters, false), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<TicketDto>>(tickets), Times.Once);
        }

        [Fact]
        public async Task Handle_NoTicketsFound_ReturnsApiOkResponseWithEmptyListAndMetaData()
        {
            var ticketParameters = new TicketParameters { PageNumber = 1, PageSize = 10 };
            var tickets = new List<Ticket>();

            var metaData = new MetaData
            {
                TotalCount = 0,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 0
            };

            var ticketsDto = new List<TicketDto>();

            var ticketsWithMetaData = new PagedList<Ticket>(tickets, metaData.TotalCount, ticketParameters.PageNumber, ticketParameters.PageSize);

            var command = new GetTicketsQuery(ticketParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Ticket.GetAllTicketsAsync(ticketParameters, false))
                .ReturnsAsync(ticketsWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<TicketDto>>(tickets)).Returns(ticketsDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<TicketDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<TicketDto>, MetaData)>;
            Assert.Empty(apiResponse.Result.Item1);
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.Ticket.GetAllTicketsAsync(ticketParameters, false), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<TicketDto>>(tickets), Times.Once);
        }
    }
}