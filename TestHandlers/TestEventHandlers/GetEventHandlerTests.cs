using AutoMapper;
using Cinema.Application.Queries.EventsQueries;
using Cinema.Application.Handlers.EventsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestEventHandlers
{
    public class GetEventHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetEventHandler _handler;

        public GetEventHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetEventHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_EventNotFound_ReturnsEventNotFoundResponse()
        {
            var eventId = Guid.NewGuid();
            var query = new GetEventQuery(eventId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Event.GetEventAsync(eventId, false))
                .ReturnsAsync((Event)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<EventNotFoundResponse>(result);
            var response = result as EventNotFoundResponse;
            Assert.Equal($"Event with id: {eventId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Event.GetEventAsync(eventId, false), Times.Once);
        }

        [Fact]
        public async Task Handle_EventFound_ReturnsApiOkResponseWithEventDto()
        {
            var eventId = Guid.NewGuid();
            var eevent = new Event { EventId = eventId, Name = "Movie Premiere" };
            var eventDto = new EventDto { EventId = eventId, Name = eevent.Name };
            var query = new GetEventQuery(eventId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Event.GetEventAsync(eventId, false))
                .ReturnsAsync(eevent);

            _mapperMock.Setup(m => m.Map<EventDto>(eevent)).Returns(eventDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<ApiOkResponse<EventDto>>(result);
            var apiResponse = result as ApiOkResponse<EventDto>;
            Assert.Equal(eevent.EventId, apiResponse.Result.EventId);
            Assert.Equal(eevent.Name, apiResponse.Result.Name);

            _repositoryMock.Verify(repo => repo.Event.GetEventAsync(eventId, false), Times.Once);
            _mapperMock.Verify(m => m.Map<EventDto>(eevent), Times.Once);
        }
    }
}