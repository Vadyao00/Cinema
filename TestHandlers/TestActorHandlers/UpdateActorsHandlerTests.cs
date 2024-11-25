using AutoMapper;
using Cinema.Application.Commands.ActorsCommands;
using Cinema.Application.Handlers.ActorsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TestHandlers.TestActorHandlers
{
    public class UpdateActorHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateActorHandler _handler;

        public UpdateActorHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateActorHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ActorNotFound_ReturnsActorNotFoundResponse()
        {
            var actorId = Guid.NewGuid();
            var actorForUpdateDto = new ActorForUpdateDto { Name = "Updated Actor" };
            var command = new UpdateActorCommand(actorId, actorForUpdateDto, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Actor.GetActorAsync(actorId, false))
                .ReturnsAsync((Actor)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ActorNotFoundResponse>(result);
            var response = result as ActorNotFoundResponse;
            Assert.Equal($"Actor with id {actorId} is not found in db.", response.Message);
        }
    }
}