using AutoMapper;
using Cinema.API;
using Cinema.Application.Commands.MoviesCommands;
using Cinema.Application.Handlers.MoviesHandlers;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestMovieHandlers
{
    public class CreateMovieHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateMovieHandler _handler;

        public CreateMovieHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });

            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(m => m.Map<Movie>(It.IsAny<MovieDto>())).Returns(new Movie());

            _handler = new CreateMovieHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        //[Fact]
        //public async Task Handle_ValidMovie_CreatesMovieAndReturnsMovieDto()
        //{
        //    // Arrange
        //    var genreId = Guid.NewGuid();
        //    var movieForCreationDto = new MovieDto { Title = "Inception", Description = "Sci-Fi Thriller" };
        //    var genre = new Genre { GenreId = genreId, Name = "Sci-Fi" };
        //    var movie = new Movie { MovieId = Guid.NewGuid(), Title = movieForCreationDto.Title, GenreId = genreId, Description = movieForCreationDto.Description };
        //    var command = new CreateMovieCommand(genreId, movieForCreationDto, false);

        //    // Настройка мока для получения жанра
        //    _repositoryMock.Setup(repo => repo.Genre.GetGenreAsync(genreId, It.IsAny<bool>())).ReturnsAsync(genre);

        //    // Настройка мока для маппинга DTO в сущность
        //    _mapperMock.Setup(m => m.Map<Movie>(movieForCreationDto)).Returns(movie);

        //    // Настройка мока для метода CreateMovieForGenre
        //    _repositoryMock.Setup(repo => repo.Movie.CreateMovieForGenre(genreId, movie));

        //    // Настройка мока для метода SaveAsync
        //    _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

        //    // Настройка мока для получения обновлённого жанра
        //    _repositoryMock.Setup(repo => repo.Genre.GetGenreAsync(genreId, false)).ReturnsAsync(genre);

        //    // Настройка мока для маппинга сущности обратно в DTO
        //    _mapperMock.Setup(m => m.Map<MovieDto>(movie)).Returns(new MovieDto
        //    {
        //        MovieId = movie.MovieId,
        //        Title = movie.Title,
        //        Description = movie.Description,
        //        GenreName = movie.Genre.Name,
        //    });

        //    // Act
        //    var result = await _handler.Handle(command, CancellationToken.None);

        //    // Assert
        //    Assert.NotNull(result);
        //    var apiResponse = Assert.IsType<ApiOkResponse<MovieDto>>(result);
        //    var resultDto = apiResponse.Result;

        //    Assert.Equal(movieForCreationDto.Title, resultDto.Title);
        //    Assert.Equal(movieForCreationDto.Description, resultDto.Description);
        //    Assert.Equal(movie.MovieId, resultDto.MovieId);

        //    // Проверка, что метод CreateMovieForGenre был вызван один раз
        //    _repositoryMock.Verify(repo => repo.Movie.CreateMovieForGenre(genreId, movie), Times.Once);

        //    // Проверка, что метод SaveAsync был вызван один раз
        //    _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);

        //    // Проверка, что маппинг сущности в DTO был вызван один раз
        //    _mapperMock.Verify(m => m.Map<Movie>(movieForCreationDto), Times.Once);
        //    _mapperMock.Verify(m => m.Map<MovieDto>(movie), Times.Once);
        //}

        //[Fact]
        //public async Task Handle_GenreNotFound_ReturnsGenreNotFoundResponse()
        //{
        //    var genreId = Guid.NewGuid();
        //    var movieForCreationDto = new MovieDto { Title = "Inception", Description = "Sci-Fi Thriller" };
        //    var command = new CreateMovieCommand(genreId, movieForCreationDto, false);

        //    _repositoryMock.Setup(repo => repo.Genre.GetGenreAsync(genreId, It.IsAny<bool>())).ReturnsAsync((Genre)null);

        //    var result = await _handler.Handle(command, CancellationToken.None);

        //    var apiResponse = Assert.IsType<GenreNotFoundResponse>(result);
        //    Assert.Equal($"Genre with id: {genreId} is not found in db.", apiResponse.Message);

        //    _repositoryMock.Verify(repo => repo.Movie.CreateMovieForGenre(It.IsAny<Guid>(), It.IsAny<Movie>()), Times.Never);
        //    _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Never);
        //}
    }
}