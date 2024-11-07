using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Exceptions;
using Cinema.Domain.Responses;
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

        public async Task<ApiBaseResponse> CreateShowtimeForMovieAsync(Guid genreId, Guid movieId, ShowtimeForCreationDto showtime, bool trackChanges)
        {
            var movie = await _repository.Movie.GetMovieAsync(genreId, movieId, trackChanges);
            if (movie is null)
                return new MovieNotFoundResponse(movieId);

            var showtimeDb = _mapper.Map<Showtime>(showtime);

            _repository.Showtime.CreateShowtimeForMovie(movieId, showtimeDb);
            await _repository.SaveAsync();

            var movieDb = await _repository.Movie.GetMovieAsync(genreId, movieId, trackChanges);
            if (movieDb is null)
                return new MovieNotFoundResponse(movieId);

            showtimeDb.Movie = movieDb;

            var showtimeToReturn = _mapper.Map<ShowtimeDto>(showtimeDb);
            return new ApiOkResponse<ShowtimeDto>(showtimeToReturn);
        }

        public async Task<ApiBaseResponse> DeleteShowtimeAsync(Guid genreId, Guid movieId, Guid Id, bool trackChanges)
        {
            var movie = await _repository.Movie.GetMovieAsync(genreId, movieId, trackChanges);
            if (movie is null)
                return new MovieNotFoundResponse(movieId);

            var showtimeForMovie = await _repository.Showtime.GetShowtimeForMovieAsync(movieId, Id, trackChanges);
            if (showtimeForMovie is null)
                return new ShowtimeNotFoundResponse(Id);

            _repository.Showtime.DeleteShowtimeForMovie(showtimeForMovie);
            await _repository.SaveAsync();

            return new ApiOkResponse<Showtime>(showtimeForMovie);
        }

        public async Task<ApiBaseResponse> GetAllShowtimesAsync(Guid genreId, Guid movieId, bool trackChanges)
        {
            var movie = await _repository.Movie.GetMovieAsync(genreId, movieId, trackChanges);
            if (movie is null)
                return new MovieNotFoundResponse(movieId);

            var showtimes = await _repository.Showtime.GetAllShowtimesForMovieAsync(movieId, trackChanges);
            var showtimesDto = _mapper.Map<IEnumerable<ShowtimeDto>>(showtimes);

            return new ApiOkResponse<IEnumerable<ShowtimeDto>>(showtimesDto);
        }

        public async Task<ApiBaseResponse> GetShowtimeAsync(Guid genreId, Guid movieId, Guid Id, bool trackChanges)
        {
            var movie = await _repository.Movie.GetMovieAsync(genreId, movieId, trackChanges);
            if (movie is null)
                return new MovieNotFoundResponse(movieId);

            var showtimeDb = await _repository.Showtime.GetShowtimeForMovieAsync(movieId, Id, trackChanges);
            if (showtimeDb is null)
                return new ShowtimeNotFoundResponse(Id);

            var showtimeDto = _mapper.Map<ShowtimeDto>(showtimeDb);
            return 
                new ApiOkResponse<ShowtimeDto>(showtimeDto);
        }

        public async Task<ApiBaseResponse> UpdateShowtimeAsync(Guid genreId, Guid movieId, Guid Id, ShowtimeForUpdateDto showtimeForUpdate, bool movTrackChanges, bool shwTrackChanges)
        {
            var movie = await _repository.Movie.GetMovieAsync(genreId, movieId, movTrackChanges);
            if (movie is null)
                return new MovieNotFoundResponse(movieId);

            var showtimeEntity = await _repository.Showtime.GetShowtimeForMovieAsync(movieId, Id, shwTrackChanges);
            if (showtimeEntity is null)
                return new ShowtimeNotFoundResponse(Id);

            _mapper.Map(showtimeForUpdate, showtimeEntity);
            await _repository.SaveAsync();

            return new ApiOkResponse<Showtime>(showtimeEntity);
        }
    }
}