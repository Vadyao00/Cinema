using AutoMapper;
using Cinema.Application.Commands.MoviesCommands;
using Cinema.Application.Handlers.MoviesHandlers;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestMovieHandlers
{
    public class DeleteMovieHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeleteMovieHandler _handler;

        public DeleteMovieHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new DeleteMovieHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_MovieNotFound_ReturnsMovieNotFoundResponse()
        {
            var movieId = Guid.NewGuid();
            var command = new DeleteMovieCommand(movieId, false);

            _repositoryMock.Setup(repo => repo.Movie.GetMovieAsync(movieId, false))
                .ReturnsAsync((Movie)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<MovieNotFoundResponse>(result);
            var response = result as MovieNotFoundResponse;
            Assert.Equal($"Movie with id: {movieId} is not found in db.", response.Message);
        }

        [Fact]
        public async Task Handle_MovieFound_DeletesMovieAndReturnsApiOkResponse()
        {
            var movieId = Guid.NewGuid();
            var movie = new Movie { MovieId = movieId, Title = "Test Movie" };
            var command = new DeleteMovieCommand(movieId, false);

            _repositoryMock.Setup(repo => repo.Movie.GetMovieAsync(movieId, false))
                .ReturnsAsync(movie);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<Movie>>(result);
            var apiResponse = result as ApiOkResponse<Movie>;
            Assert.Equal(movie, apiResponse.Result);

            _repositoryMock.Verify(repo => repo.Movie.DeleteMovie(movie), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}