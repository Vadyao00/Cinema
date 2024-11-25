using AutoMapper;
using Cinema.Application.Handlers.GenresHandlers;
using Cinema.Application.Queries.GenresQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestGenreHandlers
{
    public class GetGenresHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetGenresHandler _handler;

        public GetGenresHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetGenresHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_NoGenresFound_ReturnsApiOkResponseWithEmptyGenresAndMetaData()
        {
            var genreParameters = new GenreParameters
            {
                PageNumber = 1,
                PageSize = 5
            };
            var genres = new List<Genre>();
            var genresDto = new List<GenreDto>();
            var metaData = new MetaData
            {
                CurrentPage = 1,
                TotalPages = 0,
                PageSize = 5,
                TotalCount = 0
            };
            var pagedGenres = new PagedList<Genre>(genres, genres.Count, metaData.CurrentPage, metaData.PageSize);

            var query = new GetGenresQuery(genreParameters, false);

            _repositoryMock.Setup(repo => repo.Genre.GetAllGenresAsync(genreParameters, false))
                .ReturnsAsync(pagedGenres);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<GenreDto>>(genres))
                .Returns(genresDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<ApiOkResponse<(IEnumerable<GenreDto>, MetaData)>>(result);
            var apiResponse = result as ApiOkResponse<(IEnumerable<GenreDto>, MetaData)>;
            Assert.NotNull(apiResponse);
            Assert.Empty(apiResponse.Result.Item1);
            Assert.Equal(metaData.TotalCount, apiResponse.Result.Item2.TotalCount);

            _repositoryMock.Verify(repo => repo.Genre.GetAllGenresAsync(genreParameters, false), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<GenreDto>>(genres), Times.Once);
        }
    }
}