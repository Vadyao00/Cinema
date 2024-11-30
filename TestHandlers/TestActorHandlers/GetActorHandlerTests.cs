using AutoMapper;
using Cinema.Application.Queries.ActorsQueries;
using Cinema.Application.Handlers.ActorsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

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
            var actorId = Guid.NewGuid();
            var command = new GetActorQuery(actorId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Actor.GetActorAsync(actorId, false))
                .ReturnsAsync((Actor)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ActorNotFoundResponse>(result);
            var response = result as ActorNotFoundResponse;
            Assert.Equal($"Actor with id {actorId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Actor.GetActorAsync(actorId, false), Times.Once);
        }

        [Fact]
        public async Task Handle_ActorFound_ReturnsApiOkResponseWithActorDto()
        {
            var actorId = Guid.NewGuid();
            var actor = new Actor { ActorId = actorId, Name = "John Doe" };
            var actorDto = new ActorDto { ActorId = actorId, Name = actor.Name };
            var command = new GetActorQuery(actorId, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Actor.GetActorAsync(actorId, false))
                .ReturnsAsync(actor);

            _mapperMock.Setup(m => m.Map<ActorDto>(actor)).Returns(actorDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<ActorDto>>(result);
            var apiResponse = result as ApiOkResponse<ActorDto>;
            Assert.Equal(actor.ActorId, apiResponse.Result.ActorId);
            Assert.Equal(actor.Name, apiResponse.Result.Name);

            _repositoryMock.Verify(repo => repo.Actor.GetActorAsync(actorId, false), Times.Once);

            _mapperMock.Verify(m => m.Map<ActorDto>(actor), Times.Once);
        }
    }
}