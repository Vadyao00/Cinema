using AutoMapper;
using Cinema.Application.Commands.MoviesCommands;
using Cinema.Application.Handlers.MoviesHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestMovieHandlers
{
    public class UpdateMovieHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateMovieHandler _handler;

        public UpdateMovieHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateMovieHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_MovieNotFound_ReturnsMovieNotFoundResponse()
        {
            var movieId = Guid.NewGuid();
            var movieForUpdate = new MovieForUpdateDto { Title = "Updated Movie Title" };
            var command = new UpdateMovieCommand(movieId, movieForUpdate, false, true);

            _repositoryMock.Setup(repo => repo.Movie.GetMovieAsync(movieId, false))
                .ReturnsAsync((Movie)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<MovieNotFoundResponse>(result);
            var response = result as MovieNotFoundResponse;
            Assert.Equal($"Movie with id: {movieId} is not found in db.", response.Message);
        }
    }
}