using AutoMapper;
using Cinema.Application.Commands.GenresCommands;
using Cinema.Application.Handlers.GenresHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestGenreHandlers
{
    public class UpdateGenreHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateGenreHandler _handler;

        public UpdateGenreHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new UpdateGenreHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_GenreNotFound_ReturnsGenreNotFoundResponse()
        {
            var genreId = Guid.NewGuid();
            var genreForUpdateDto = new GenreForUpdateDto { Name = "Updated Genre Name" };
            var command = new UpdateGenreCommand(genreId, genreForUpdateDto, false);

            _repositoryMock.Setup(repo => repo.Genre.GetGenreAsync(genreId, false))
                .ReturnsAsync((Genre?)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<GenreNotFoundResponse>(result);
            var notFoundResponse = result as GenreNotFoundResponse;
            Assert.NotNull(notFoundResponse);
            Assert.Equal($"Genre with id: {genreId} is not found in db.", notFoundResponse.Message);

            _repositoryMock.Verify(repo => repo.Genre.GetGenreAsync(genreId, false), Times.Once);
            _mapperMock.Verify(m => m.Map(It.IsAny<GenreForUpdateDto>(), It.IsAny<Genre>()), Times.Never);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        }
    }
}