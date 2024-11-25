using AutoMapper;
using Cinema.Application.Commands.GenresCommands;
using Cinema.Application.Handlers.GenresHandlers;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;
using Xunit;

namespace TestHandlers.TestGenreHandlers
{
    public class DeleteGenreHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeleteGenreHandler _handler;

        public DeleteGenreHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();

            _mapperMock = new Mock<IMapper>();
            _handler = new DeleteGenreHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_GenreNotFound_ReturnsGenreNotFoundResponse()
        {
            var genreId = Guid.NewGuid();
            var command = new DeleteGenreCommand(genreId, false);

            _repositoryMock.Setup(repo => repo.Genre.GetGenreAsync(genreId, false))
                .ReturnsAsync((Genre)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<GenreNotFoundResponse>(result);
            var response = result as GenreNotFoundResponse;
            Assert.Equal($"Genre with id: {genreId} is not found in db.", response.Message);
        }

        [Fact]
        public async Task Handle_GenreFound_DeletesGenreAndReturnsApiOkResponse()
        {
            var genreId = Guid.NewGuid();
            var genre = new Genre { GenreId = genreId, Name = "Action" };
            var command = new DeleteGenreCommand(genreId, false);

            _repositoryMock.Setup(repo => repo.Genre.GetGenreAsync(genreId, false))
                .ReturnsAsync(genre);

            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<ApiOkResponse<Genre>>(result);
            var apiResponse = result as ApiOkResponse<Genre>;
            Assert.Equal(genre.Name, apiResponse.Result.Name);

            _repositoryMock.Verify(repo => repo.Genre.DeleteGenre(genre), Times.Once);

            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
        }
    }
}