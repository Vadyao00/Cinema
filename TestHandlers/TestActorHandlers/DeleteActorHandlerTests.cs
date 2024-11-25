using Cinema.Application.Commands.ActorsCommands;
using Cinema.Application.Handlers.ActorsHandlers;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestActorHandlers
{
    public class DeleteActorHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly DeleteActorHandler _handler;

        public DeleteActorHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _handler = new DeleteActorHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ActorNotFound_ReturnsActorNotFoundResponse()
        {
            var actorId = Guid.NewGuid();
            var command = new DeleteActorCommand(actorId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Actor.GetActorAsync(actorId, false))
                .ReturnsAsync((Actor)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ActorNotFoundResponse>(result);
            var response = result as ActorNotFoundResponse;
            Assert.Equal($"Actor with id {actorId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Actor.DeleteActor(It.IsAny<Actor>()), Times.Never);

            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ActorFound_DeletesActorAndReturnsApiOkResponse()
        {
            var actorId = Guid.NewGuid();
            var actor = new Actor { ActorId = actorId, Name = "John Doe" };
            var command = new DeleteActorCommand(actorId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Actor.GetActorAsync(actorId, false))
                .ReturnsAsync(actor);

            _repositoryMock.Setup(repo => repo.Actor.DeleteActor(actor));

            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<Actor>>(result);
            var apiResponse = result as ApiOkResponse<Actor>;
            Assert.Equal(actor.ActorId, apiResponse.Result.ActorId);
            Assert.Equal(actor.Name, apiResponse.Result.Name);

            _repositoryMock.Verify(repo => repo.Actor.DeleteActor(actor), Times.Once);

            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}