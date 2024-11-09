using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Cinema.LoggerService;
using Contracts.IRepositories;
using Contracts.IServices;

namespace Cinema.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public MovieService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> CreateMovieForGenreAsync(Guid genreId, MovieForCreationDto movie, bool trackChanges)
        {
            var genre = await _repository.Genre.GetGenreAsync(genreId, trackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(genreId);

            var movieDb = _mapper.Map<Movie>(movie);

            _repository.Movie.CreateMovieForGenre(genreId, movieDb);
            await _repository.SaveAsync();

            genre = await _repository.Genre.GetGenreAsync(genreId, false);
            movieDb.Genre = genre;

            var movieToReturn = _mapper.Map<MovieDto>(movieDb);
            return new ApiOkResponse<MovieDto>(movieToReturn);
        }

        public async Task<ApiBaseResponse> DeleteMovieAsync(Guid genreId, Guid Id, bool trackChanges)
        {
            var genre = await _repository.Genre.GetGenreAsync(genreId, trackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(genreId);

            var movieDb = await _repository.Movie.GetMovieAsync(genreId, Id, trackChanges);
            if (movieDb is null)
                return new MovieNotFoundResponse(Id);

            _repository.Movie.DeleteMovie(movieDb);
            await _repository.SaveAsync();

            return new ApiOkResponse<Movie>(movieDb);
        }

        public async Task<ApiBaseResponse> GetAllMoviesAsync(MovieParameters movieParameters, Guid genreId, bool trackChanges)
        {
            var genre = await _repository.Genre.GetGenreAsync(genreId, trackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(genreId);

            var moviesWithMetaData = await _repository.Movie.GetAllMoviesForGenreAsync(movieParameters, genreId, trackChanges);
            var moviesDto = _mapper.Map<IEnumerable<MovieDto>>(moviesWithMetaData);

            return new ApiOkResponse<(IEnumerable<MovieDto>, MetaData)>((moviesDto, moviesWithMetaData.MetaData));
        }

        public async Task<ApiBaseResponse> GetMovieAsync(Guid genreId, Guid Id, bool trackChanges)
        {
            var genre = await _repository.Genre.GetGenreAsync(genreId, trackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(genreId);

            var movieDb = await _repository.Movie.GetMovieAsync(genreId, Id, trackChanges);
            if (movieDb is null)
                return new MovieNotFoundResponse(Id);

            var movieDto = _mapper.Map<MovieDto>(movieDb);
            return new ApiOkResponse<MovieDto>(movieDto);
        }

        public async Task<ApiBaseResponse> UpdateMovieForGenreAsync(Guid genreId, Guid Id, MovieForUpdateDto movieForUpdate, bool genrTrackChanges, bool movTrackChanges)
        {
            var genre = await _repository.Genre.GetGenreAsync(genreId, genrTrackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(genreId);

            var movieDb = await _repository.Movie.GetMovieAsync(genreId, Id, movTrackChanges);
            if (movieDb is null)
                return new MovieNotFoundResponse(Id);

            _mapper.Map(movieForUpdate, movieDb);
            await _repository.SaveAsync();

            return new ApiOkResponse<Movie>(movieDb);
        }
    }
}