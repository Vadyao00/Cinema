using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Exceptions;
using Cinema.LoggerService;
using Contracts.IRepositories;
using Contracts.IServices;
using System.ComponentModel.Design;

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

        public async Task<MovieDto> CreateMovieForGenreAsync(Guid genreId, MovieForCreationDto movie, bool trackChanges)
        {
            await CheckIfGenreExists(genreId, trackChanges);

            var movieDb = _mapper.Map<Movie>(movie);

            _repository.Movie.CreateMovieForGenre(genreId, movieDb);
            await _repository.SaveAsync();

            var genre = await GetGenreModel(genreId, trackChanges: false);
            movieDb.Genre = genre;

            var movieToReturn = _mapper.Map<MovieDto>(movieDb);
            return movieToReturn;
        }

        public async Task DeleteMovieAsync(Guid genreId, Guid Id, bool trackChanges)
        {
            await CheckIfGenreExists(genreId, trackChanges);

            var movieForGenre = await GetMovieForGenreAndCheckIfItExists(genreId, Id, trackChanges);

            _repository.Movie.DeleteMovie(movieForGenre);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<MovieDto>> GetAllMoviesAsync(Guid genreId, bool trackChanges)
        {
            await CheckIfGenreExists(genreId, trackChanges);

            var movies = await _repository.Movie.GetAllMoviesForGenreAsync(genreId, trackChanges);
            var moviesDto = _mapper.Map<IEnumerable<MovieDto>>(movies);

            return moviesDto;
        }

        public async Task<MovieDto> GetMovieAsync(Guid genreId, Guid Id, bool trackChanges)
        {
            await CheckIfGenreExists(genreId, trackChanges);

            var movieDb = await GetMovieForGenreAndCheckIfItExists(genreId, Id, trackChanges);

            var movieDto = _mapper.Map<MovieDto>(movieDb);
            return movieDto;
        }

        public async Task UpdateMovieForGenreAsync(Guid genreId, Guid Id, MovieForUpdateDto movieForUpdate, bool genrTrackChanges, bool movTrackChanges)
        {
            await CheckIfGenreExists(genreId, genrTrackChanges);

            var movidEntity = await GetMovieForGenreAndCheckIfItExists(genreId, Id, movTrackChanges);

            _mapper.Map(movieForUpdate, movidEntity);
            await _repository.SaveAsync();
        }

        private async Task CheckIfGenreExists(Guid genreId, bool trackChanges)
        {
            var genre = await _repository.Genre.GetGenreAsync(genreId, trackChanges);
            if (genre is null)
                throw new GenreNotFoundException(genreId);
        }

        private async Task<Genre> GetGenreModel(Guid genreId, bool trackChanges)
        {
            var genre = await _repository.Genre.GetGenreAsync(genreId, trackChanges);
            if (genre is null)
                throw new GenreNotFoundException(genreId);
            return genre;
        }

        private async Task<Movie> GetMovieForGenreAndCheckIfItExists(Guid genreId, Guid id, bool trackChanges)
        {
            var movieDb = await _repository.Movie.GetMovieAsync(genreId, id, trackChanges);
            if (movieDb is null)
                throw new MovieNotFoundException(id);

            return movieDb;
        }
    }
}