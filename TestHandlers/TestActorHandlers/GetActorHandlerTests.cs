using AutoMapper;
using Cinema.Application.Queries.ActorsQueries;
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
    public class GetActorHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetActorHandler _handler;

        public GetActorHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetActorHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ActorNotFound_ReturnsActorNotFoundResponse()
        {
            // Arrange
            var actorId = Guid.NewGuid();
            var command = new GetActorQuery(actorId, TrackChanges: false);

            // Настройка мока для получения актера
            _repositoryMock.Setup(repo => repo.Actor.GetActorAsync(actorId, false))
                .ReturnsAsync((Actor)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ActorNotFoundResponse>(result);
            var response = result as ActorNotFoundResponse;
            Assert.Equal($"Actor with id {actorId} is not found in db.", response.Message);

            // Проверка, что метод GetActorAsync был вызван один раз
            _repositoryMock.Verify(repo => repo.Actor.GetActorAsync(actorId, false), Times.Once);
        }

        [Fact]
        public async Task Handle_ActorFound_ReturnsApiOkResponseWithActorDto()
        {
            // Arrange
            var actorId = Guid.NewGuid();
            var actor = new Actor { ActorId = actorId, Name = "John Doe" };
            var actorDto = new ActorDto { ActorId = actorId, Name = actor.Name };
            var command = new GetActorQuery(actorId, TrackChanges: false);

            // Настройка мока для получения актера
            _repositoryMock.Setup(repo => repo.Actor.GetActorAsync(actorId, false))
                .ReturnsAsync(actor);

            // Настройка мока для маппинга Actor в ActorDto
            _mapperMock.Setup(m => m.Map<ActorDto>(actor)).Returns(actorDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ApiOkResponse<ActorDto>>(result);
            var apiResponse = result as ApiOkResponse<ActorDto>;
            Assert.Equal(actor.ActorId, apiResponse.Result.ActorId);
            Assert.Equal(actor.Name, apiResponse.Result.Name);

            // Проверка, что метод GetActorAsync был вызван один раз
            _repositoryMock.Verify(repo => repo.Actor.GetActorAsync(actorId, false), Times.Once);

            // Проверка, что метод маппинга был вызван один раз
            _mapperMock.Verify(m => m.Map<ActorDto>(actor), Times.Once);
        }
    }
}