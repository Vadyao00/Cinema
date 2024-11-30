using AutoMapper;
using Cinema.Application.Commands.ShowtimesCommands;
using Cinema.Application.Handlers.ShowtimesHandlers;
using Cinema.Controllers.Extensions;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using Moq;

namespace TestHandlers.TestShowtimeHandlers
{
    public class CreateShowtimeHandlerTests
    {
        private readonly Mock<IRepositoryManager> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateShowtimeHandler _handler;

        public CreateShowtimeHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryManager>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateShowtimeHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ValidShowtime_CreatesShowtimeAndReturnsShowtimeDto()
        {
            var showtimeForCreationDto = new ShowtimeForCreationDto { StartTime = TimeOnly.FromDateTime(DateTime.Now), EndTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(2)) };
            var movie = new Movie { MovieId = Guid.NewGuid(), Title = "Some Movie" };
            var showtime = new Showtime { ShowtimeId = Guid.NewGuid(), StartTime = showtimeForCreationDto.StartTime, EndTime = showtimeForCreationDto.EndTime };
            var command = new CreateShowtimeCommand(movie.MovieId, showtimeForCreationDto, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Movie.GetMovieAsync(movie.MovieId, false)).ReturnsAsync(movie);
            _mapperMock.Setup(m => m.Map<Showtime>(showtimeForCreationDto)).Returns(showtime);
            _mapperMock.Setup(m => m.Map<ShowtimeDto>(showtime)).Returns(new ShowtimeDto { ShowtimeId = showtime.ShowtimeId, StartTime = showtime.StartTime, EndTime = showtime.EndTime });

            _repositoryMock.Setup(repo => repo.Showtime.CreateShowtimeForMovie(movie.MovieId, showtime));
            _repositoryMock.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(showtimeForCreationDto.StartTime, result.GetResult<ShowtimeDto>().StartTime);
            Assert.Equal(showtimeForCreationDto.EndTime, result.GetResult<ShowtimeDto>().EndTime);
            Assert.Equal(showtime.ShowtimeId, result.GetResult<ShowtimeDto>().ShowtimeId);

            _repositoryMock.Verify(repo => repo.Showtime.CreateShowtimeForMovie(movie.MovieId, showtime), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<Showtime>(showtimeForCreationDto), Times.Once);
            _mapperMock.Verify(m => m.Map<ShowtimeDto>(showtime), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidMovieId_ReturnsMovieNotFoundResponse()
        {
            var showtimeForCreationDto = new ShowtimeForCreationDto { StartTime =TimeOnly.FromDateTime(DateTime.Now), EndTime =TimeOnly.FromDateTime(DateTime.Now.AddHours(2)) };
            var command = new CreateShowtimeCommand(Guid.NewGuid(), showtimeForCreationDto, TrackChanges: false);

            _repositoryMock.Setup(repo => repo.Movie.GetMovieAsync(command.MovieId, false)).ReturnsAsync((Movie)null);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsType<MovieNotFoundResponse>(result);
            var response = result as MovieNotFoundResponse;
            Assert.Equal($"Movie with id: {command.MovieId} is not found in db.", response.Message);

            _repositoryMock.Verify(repo => repo.Movie.GetMovieAsync(command.MovieId, false), Times.Once);
        }
    }
}