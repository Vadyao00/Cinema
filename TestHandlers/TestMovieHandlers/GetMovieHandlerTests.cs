using AutoMapper;
using Cinema.Application.Queries.MoviesQueries;
using Cinema.Application.Handlers.MoviesHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestMovieHandlers
{
    public class GetMovieHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetMovieHandler _handler;

        public GetMovieHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetMovieHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_MovieNotFound_ReturnsMovieNotFoundResponse()
        {
            var movieId = Guid.NewGuid();
            var command = new GetMovieQuery(movieId, false);

            _repositoryMock.Setup(repo => repo.Movie.GetMovieAsync(movieId, false))
                .ReturnsAsync((Movie)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<MovieNotFoundResponse>(result);
            var response = result as MovieNotFoundResponse;
            Assert.Equal($"Movie with id: {movieId} is not found in db.", response.Message);
        }

        [Fact]
        public async Task Handle_MovieFound_ReturnsApiOkResponse()
        {
            var movieId = Guid.NewGuid();
            var movie = new Movie { MovieId = movieId, Title = "Test Movie" };
            var movieDto = new MovieDto { Title = "Test Movie" };
            var command = new GetMovieQuery(movieId, false);

            _repositoryMock.Setup(repo => repo.Movie.GetMovieAsync(movieId, false))
                .ReturnsAsync(movie);

            _mapperMock.Setup(m => m.Map<MovieDto>(movie)).Returns(movieDto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<MovieDto>>(result);
            var apiResponse = result as ApiOkResponse<MovieDto>;
            Assert.Equal(movieDto.Title, apiResponse.Result.Title);

            _repositoryMock.Verify(repo => repo.Movie.GetMovieAsync(movieId, false), Times.Once);
        }
    }
}