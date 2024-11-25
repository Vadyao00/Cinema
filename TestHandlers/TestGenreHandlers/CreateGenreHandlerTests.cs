using AutoMapper;
using Cinema.Application.Commands.GenresCommands;
using Cinema.Application.Handlers.GenresHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Moq;
using Xunit;

namespace TestHandlers.TestGenreHandlers
{
    public class CreateGenreHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateGenreHandler _handler;

        public CreateGenreHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();

            _mapperMock = new Mock<IMapper>();
            _handler = new CreateGenreHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidGenre_CreatesGenreAndReturnsGenreDto()
        {
            var genreForCreationDto = new GenreForCreationDto { Name = "Action" };
            var genre = new Genre { GenreId = Guid.NewGuid(), Name = genreForCreationDto.Name };
            var command = new CreateGenreCommand(genreForCreationDto);

            _mapperMock.Setup(m => m.Map<Genre>(genreForCreationDto)).Returns(genre);

            _mapperMock.Setup(m => m.Map<GenreDto>(genre)).Returns(new GenreDto { GenreId = genre.GenreId, Name = genre.Name });

            _repositoryMock.Setup(repo => repo.Genre.CreateGenre(genre));

            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(genreForCreationDto.Name, result.Name);
            Assert.Equal(genre.GenreId, result.GenreId);

            _repositoryMock.Verify(repo => repo.Genre.CreateGenre(genre), Times.Once);

            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);

            _mapperMock.Verify(m => m.Map<Genre>(genreForCreationDto), Times.Once);
            _mapperMock.Verify(m => m.Map<GenreDto>(genre), Times.Once);
        }
    }
}