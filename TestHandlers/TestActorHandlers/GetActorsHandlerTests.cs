using AutoMapper;
using Cinema.Application.Queries.ActorsQueries;
using Cinema.Application.Handlers.ActorsHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestActorHandlers
{
    public class GetActorsHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetActorsHandler _handler;

        public GetActorsHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetActorsHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ActorsFound_ReturnsApiOkResponseWithActorDtoAndMetaData()
        {
            // Arrange
            var actorParameters = new ActorParameters { PageNumber = 1, PageSize = 10 };
            var actors = new List<Actor>
            {
                new Actor { ActorId = Guid.NewGuid(), Name = "John Doe" },
                new Actor { ActorId = Guid.NewGuid(), Name = "Jane Smith" }
            };

            var metaData = new MetaData
            {
                TotalCount = 2,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 1
            };

            var actorsDto = new List<ActorDto>
            {
                new ActorDto { ActorId = actors[0].ActorId, Name = actors[0].Name },
                new ActorDto { ActorId = actors[1].ActorId, Name = actors[1].Name }
            };

            var actorsWithMetaData = new PagedList<Actor>(actors, metaData.TotalCount, actorParameters.PageNumber, actorParameters.PageSize);

            var command = new GetActorsQuery(actorParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Actor.GetAllActorsAsync(actorParameters, false))
                .ReturnsAsync(actorsWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<ActorDto>>(actors)).Returns(actorsDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<ActorDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<ActorDto>, MetaData)>;
            Assert.Equal(2, apiResponse.Result.Item1.Count());
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.Actor.GetAllActorsAsync(actorParameters, false), Times.Once);

            _mapperMock.Verify(m => m.Map<IEnumerable<ActorDto>>(actors), Times.Once);
        }

        [Fact]
        public async Task Handle_NoActorsFound_ReturnsApiOkResponseWithEmptyListAndMetaData()
        {
            var actorParameters = new ActorParameters { PageNumber = 1, PageSize = 10 };
            var actors = new List<Actor>();

            var metaData = new MetaData
            {
                TotalCount = 0,
                PageSize = 10,
                CurrentPage = 1,
                TotalPages = 0
            };

            var actorsDto = new List<ActorDto>();

            var actorsWithMetaData = new PagedList<Actor>(actors, metaData.TotalCount, actorParameters.PageNumber, actorParameters.PageSize);

            var command = new GetActorsQuery(actorParameters, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Actor.GetAllActorsAsync(actorParameters, false))
                .ReturnsAsync(actorsWithMetaData);

            _mapperMock.Setup(m => m.Map<IEnumerable<ActorDto>>(actors)).Returns(actorsDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<ActorDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<ActorDto>, MetaData)>;
            Assert.Empty(apiResponse.Result.Item1);
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);
            Assert.Equal(metaData.PageSize, apiResponse.Result.Item2.PageSize);
            Assert.Equal(metaData.CurrentPage, apiResponse.Result.Item2.CurrentPage);
            Assert.Equal(metaData.TotalPages, apiResponse.Result.Item2.TotalPages);

            _repositoryMock.Verify(repo => repo.Actor.GetAllActorsAsync(actorParameters, false), Times.Once);

            _mapperMock.Verify(m => m.Map<IEnumerable<ActorDto>>(actors), Times.Once);
        }
    }
}