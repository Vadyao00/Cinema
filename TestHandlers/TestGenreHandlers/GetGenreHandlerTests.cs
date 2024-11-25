using AutoMapper;
using Cinema.Application.Handlers.GenresHandlers;
using Cinema.Application.Queries.GenresQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestGenreHandlers
{
    public class GetGenreHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetGenreHandler _handler;

        public GetGenreHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetGenreHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_GenreNotFound_ReturnsGenreNotFoundResponse()
        {
            var genreId = Guid.NewGuid();
            var query = new GetGenreQuery(genreId, false);

            _repositoryMock.Setup(repo => repo.Genre.GetGenreAsync(genreId, false))
                .ReturnsAsync((Genre)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<GenreNotFoundResponse>(result);
            var response = result as GenreNotFoundResponse;
            Assert.NotNull(response);
            Assert.Equal($"Genre with id: {genreId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Genre.GetGenreAsync(genreId, false), Times.Once);
        }

        [Fact]
        public async Task Handle_GenreFound_ReturnsApiOkResponseWithGenreDto()
        {
            var genreId = Guid.NewGuid();
            var genre = new Genre { GenreId = genreId, Name = "Comedy" };
            var genreDto = new GenreDto { GenreId = genreId, Name = "Comedy" };
            var query = new GetGenreQuery(genreId, false);

            _repositoryMock.Setup(repo => repo.Genre.GetGenreAsync(genreId, false))
                .ReturnsAsync(genre);

            _mapperMock.Setup(mapper => mapper.Map<GenreDto>(genre))
                .Returns(genreDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.IsType<ApiOkResponse<GenreDto>>(result);
            var apiResponse = result as ApiOkResponse<GenreDto>;
            Assert.NotNull(apiResponse);
            Assert.Equal(genreDto, apiResponse.Result);

            _repositoryMock.Verify(repo => repo.Genre.GetGenreAsync(genreId, false), Times.Once);

            _mapperMock.Verify(mapper => mapper.Map<GenreDto>(genre), Times.Once);
        }
    }
}