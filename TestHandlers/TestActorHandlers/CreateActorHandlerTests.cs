using AutoMapper;
using Cinema.Application.Commands.ActorsCommands;
using Cinema.Application.Handlers.ActorsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Moq;
using Xunit;

namespace TestHandlers.TestActorHandlers
{
    public class CreateActorHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateActorHandler _handler;

        public CreateActorHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();

            _mapperMock = new Mock<IMapper>();
            _handler = new CreateActorHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidActor_CreatesActorAndReturnsActorDto()
        {
            var actorForCreationDto = new ActorForCreationDto { Name = "John Doe" };
            var actor = new Actor { ActorId = Guid.NewGuid(), Name = actorForCreationDto.Name };
            var command = new CreateActorCommand(actorForCreationDto);

            _mapperMock.Setup(m => m.Map<Actor>(actorForCreationDto)).Returns(actor);
            _mapperMock.Setup(m => m.Map<ActorDto>(actor)).Returns(new ActorDto { ActorId = actor.ActorId, Name = actor.Name });

            _repositoryMock.Setup(repo => repo.Actor.CreateActor(actor));
            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(actorForCreationDto.Name, result.Name);
            Assert.Equal(actor.ActorId, result.ActorId);

            _repositoryMock.Verify(repo => repo.Actor.CreateActor(actor), Times.Once);

            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);

            _mapperMock.Verify(m => m.Map<Actor>(actorForCreationDto), Times.Once);
            _mapperMock.Verify(m => m.Map<ActorDto>(actor), Times.Once);
        }
    }
}