using Cinema.Application.Commands.EventsCommands;
using Cinema.Application.Handlers.EventsHandlers;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestEventHandlers
{
    public class DeleteEventHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly DeleteEventHandler _handler;

        public DeleteEventHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _handler = new DeleteEventHandler(_repositoryMock.Object, null);
        }

        [Fact]
        public async Task Handle_EventNotFound_ReturnsEventNotFoundResponse()
        {
            var eventId = Guid.NewGuid();
            var command = new DeleteEventCommand(eventId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Event.GetEventAsync(eventId, false))
                .ReturnsAsync((Event)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<EventNotFoundResponse>(result);
            var response = result as EventNotFoundResponse;
            Assert.Equal($"Event with id: {eventId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Event.DeleteEvent(It.IsAny<Event>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_EventFound_DeletesEventAndReturnsApiOkResponse()
        {
            var eventId = Guid.NewGuid();
            var eevent = new Event { EventId = eventId, Name = "Movie Premiere" };
            var command = new DeleteEventCommand(eventId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Event.GetEventAsync(eventId, false))
                .ReturnsAsync(eevent);

            _repositoryMock.Setup(repo => repo.Event.DeleteEvent(eevent));
            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<Event>>(result);
            var apiResponse = result as ApiOkResponse<Event>;
            Assert.Equal(eevent.EventId, apiResponse.Result.EventId);
            Assert.Equal(eevent.Name, apiResponse.Result.Name);

            _repositoryMock.Verify(repo => repo.Event.DeleteEvent(eevent), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}