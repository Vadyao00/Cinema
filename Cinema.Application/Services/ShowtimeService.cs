using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Exceptions;
using Cinema.LoggerService;
using Contracts.IRepositories;
using Contracts.IServices;

namespace Cinema.Application.Services
{
    public class ShowtimeService : IShowtimeService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ShowtimeService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ShowtimeDto> CreateShowtimeForMovieAsync(Guid genreId, Guid movieId, ShowtimeForCreationDto showtime, bool trackChanges)
        {
            await CheckIfMovieExists(genreId, movieId, trackChanges);

            var showtimeDb = _mapper.Map<Showtime>(showtime);

            _repository.Showtime.CreateShowtimeForMovie(movieId, showtimeDb);
            await _repository.SaveAsync();

            var showtimeToReturn = _mapper.Map<ShowtimeDto>(showtimeDb);
            return showtimeToReturn;
        }

        public async Task DeleteShowtimeAsync(Guid genreId, Guid movieId, Guid Id, bool trackChanges)
        {
            await CheckIfMovieExists(genreId, movieId, trackChanges);

            var showtimeForMovie = await GetShowtimeForMovieAndCheckIfItExists(movieId, Id, trackChanges);

            _repository.Showtime.DeleteShowtimeForMovie(showtimeForMovie);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<ShowtimeDto>> GetAllShowtimesAsync(Guid genreId, Guid movieId, bool trackChanges)
        {
            await CheckIfMovieExists(genreId, movieId, trackChanges);

            var showtimes = await _repository.Showtime.GetAllShowtimesForMovieAsync(movieId, trackChanges);
            var showtimesDto = _mapper.Map<IEnumerable<ShowtimeDto>>(showtimes);

            return showtimesDto;
        }

        public async Task<ShowtimeDto> GetShowtimeAsync(Guid genreId, Guid movieId, Guid Id, bool trackChanges)
        {
            await CheckIfMovieExists(genreId, movieId, trackChanges);

            var showtimeDb = await GetShowtimeForMovieAndCheckIfItExists(movieId, Id, trackChanges);

            var showtimeDto = _mapper.Map<ShowtimeDto>(showtimeDb);
            return showtimeDto;
        }

        public async Task UpdateShowtimeAsync(Guid genreId, Guid movieId, Guid Id, ShowtimeForUpdateDto showtimeForUpdate, bool movTrackChanges, bool shwTrackChanges)
        {
            await CheckIfMovieExists(genreId, movieId, movTrackChanges);

            var showtimeEntity = await GetShowtimeForMovieAndCheckIfItExists(movieId, Id, shwTrackChanges);

            _mapper.Map(showtimeForUpdate, showtimeEntity);
            await _repository.SaveAsync();
        }

        private async Task CheckIfMovieExists(Guid genreId, Guid movieId, bool trackChanges)
        {
            var movie = await _repository.Movie.GetMovieAsync(genreId, movieId, trackChanges);
            if (movie is null)
                throw new MovieNotFoundException(movieId);
        }

        private async Task<Showtime> GetShowtimeForMovieAndCheckIfItExists(Guid movieId, Guid id, bool trackChanges)
        {
            var showtimeDb = await _repository.Showtime.GetShowtimeForMovieAsync(movieId, id, trackChanges);
            if (showtimeDb is null)
                throw new ShowtimeNotFoundException(id);

            return showtimeDb;
        }
    }
}