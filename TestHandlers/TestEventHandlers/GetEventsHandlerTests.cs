using AutoMapper;
using Cinema.Application.Queries.EventsQueries;
using Cinema.Application.Handlers.EventsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestEventHandlers
{
    public class GetEventsHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetEventsHandler _handler;

        public GetEventsHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetEventsHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_InvalidTicketPriceRange_ReturnsMaxTicketPriceBadRequestResponse()
        {
            var eventParameters = new EventParameters
            {
                MinTicketPrice = 100,
                MaxTicketPrice = 50
            };
            var query = new GetEventsQuery(eventParameters, TrackChanges: false);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<MaxTicketPriceBadRequestPesponse>(result);
        }

        [Fact]
        public async Task Handle_InvalidTimeRange_ReturnsTimeRangeBadRequestResponse()
        {
            var eventParameters = new EventParameters
            {
                StartTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(1)),
                EndTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(-1))
            };
            var query = new GetEventsQuery(eventParameters, TrackChanges: false);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<TimeRangeBadRequestResponse>(result);
        }

        [Fact]
        public async Task Handle_EventsFound_ReturnsApiOkResponseWithEventDtoAndMetaData()
        {
            var eventParameters = new EventParameters { PageNumber = 1, PageSize = 10 };
            var events = new List<Event>
            {
                new Event { EventId = Guid.NewGuid(), Name = "Concert" },
                new Event { EventId = Guid.NewGuid(), Name = "Theater Play" }
            };

            var metaData = new MetaData
            {
                TotalCount = 2,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 1
            };

            var eventsDto = new List<EventDto>
            {
                new EventDto { EventId = events[0].EventId, Name = events[0].Name },
                new EventDto { EventId = events[1].EventId, Name = events[1].Name }
            };

            var eventsWithMetaData = new PagedList<Event>(events, metaData.TotalCount, eventParameters.PageNumber, eventParameters.PageSize);

            var query = new GetEventsQuery(eventParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Event.GetAllEventsAsync(eventParameters, false))
                .ReturnsAsync(eventsWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<EventDto>>(events)).Returns(eventsDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<EventDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<EventDto>, MetaData)>;
            Assert.Equal(2, apiResponse.Result.Item1.Count());
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.Event.GetAllEventsAsync(eventParameters, false), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<EventDto>>(events), Times.Once);
        }

        [Fact]
        public async Task Handle_NoEventsFound_ReturnsApiOkResponseWithEmptyListAndMetaData()
        {
            var eventParameters = new EventParameters { PageNumber = 1, PageSize = 10 };
            var events = new List<Event>();

            var metaData = new MetaData
            {
                TotalCount = 0,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 0
            };

            var eventsDto = new List<EventDto>();

            var eventsWithMetaData = new PagedList<Event>(events, metaData.TotalCount, eventParameters.PageNumber, eventParameters.PageSize);

            var query = new GetEventsQuery(eventParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Event.GetAllEventsAsync(eventParameters, false))
                .ReturnsAsync(eventsWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<EventDto>>(events)).Returns(eventsDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<EventDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<EventDto>, MetaData)>;
            Assert.Empty(apiResponse.Result.Item1);
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.Event.GetAllEventsAsync(eventParameters, false), Times.Once);
            _mapperMock.Verify(m => m.Map<IEnumerable<EventDto>>(events), Times.Once);
        }
    }
}