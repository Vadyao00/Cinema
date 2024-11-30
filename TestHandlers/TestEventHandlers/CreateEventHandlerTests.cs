using AutoMapper;
using Cinema.Application.Commands.EventsCommands;
using Cinema.Application.Handlers.EventsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestEventHandlers
{
    public class CreateEventHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateEventHandler _handler;

        public CreateEventHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateEventHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidEvent_CreatesEventAndReturnsEventDto()
        {
            var eventForCreationDto = new EventForCreationDto { Name = "Movie Premiere", Date = DateOnly.FromDateTime(DateTime.Now) };
            var eventEntity = new Event { EventId = Guid.NewGuid(), Name = eventForCreationDto.Name, Date = (DateOnly)eventForCreationDto.Date };
            var command = new CreateEventCommand(eventForCreationDto);

            _mapperMock.Setup(m => m.Map<Event>(eventForCreationDto)).Returns(eventEntity);
            _mapperMock.Setup(m => m.Map<EventDto>(eventEntity)).Returns(new EventDto { EventId = eventEntity.EventId, Name = eventEntity.Name, Date = eventEntity.Date });

            _repositoryMock.Setup(repo => repo.Event.CreateEvent(eventEntity));
            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(eventForCreationDto.Name, result.Name);
            Assert.Equal(eventEntity.EventId, result.EventId);
            Assert.Equal(eventEntity.Date, result.Date);

            _repositoryMock.Verify(repo => repo.Event.CreateEvent(eventEntity), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<Event>(eventForCreationDto), Times.Once);
            _mapperMock.Verify(m => m.Map<EventDto>(eventEntity), Times.Once);
        }
    }
}