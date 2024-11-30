using AutoMapper;
using Cinema.Application.Commands.EventsCommands;
using Cinema.Application.Handlers.EventsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestEventHandlers
{
    public class UpdateEventHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateEventHandler _handler;

        public UpdateEventHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateEventHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_EventNotFound_ReturnsEventNotFoundResponse()
        {
            var eventId = Guid.NewGuid();
            var eventForUpdateDto = new EventForUpdateDto { Name = "Updated Event" };
            var command = new UpdateEventCommand(eventId, eventForUpdateDto, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Event.GetEventAsync(eventId, false))
                .ReturnsAsync((Event)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<EventNotFoundResponse>(result);
            var response = result as EventNotFoundResponse;
            Assert.Equal($"Event with id: {eventId} is not found in db.", response.Message);
        }
    }
}