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
    public class GenreService : IGenreService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public GenreService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<GenreDto> CreateGenreAsync(GenreForCreationDto genre)
        {
            var genreEntity = _mapper.Map<Genre>(genre);

            _repository.Genre.CreateGenre(genreEntity);
            await _repository.SaveAsync();

            var genreToReturn = _mapper.Map<GenreDto>(genreEntity);

            return genreToReturn;
        }

        public async Task DeleteGenreAsync(Guid genreId, bool trackChanges)
        {
            var genre = await GetGenreAndCheckIfItExists(genreId, trackChanges);

            _repository.Genre.DeleteGenre(genre);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<GenreDto>> GetAllGenresAsync(bool trackChanges)
        {
            var genres = await _repository.Genre.GetAllGenresAsync(trackChanges);
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genres);

            return genresDto;
        }

        public async Task<IEnumerable<GenreDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametrBadRequestException();

            var genreEntities = await _repository.Genre.GetByIdsAsync(ids, trackChanges);
            if (ids.Count() != genreEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var genresToReturn = _mapper.Map<IEnumerable<GenreDto>>(genreEntities);

            return genresToReturn;
        }

        public async Task<GenreDto> GetGenreAsync(Guid genreId, bool trackChanges)
        {
            var genre = await GetGenreAndCheckIfItExists(genreId, trackChanges);

            var genreDto = _mapper.Map<GenreDto>(genre);

            return genreDto;
        }

        public async Task UpdateGenreAsync(Guid genreId, GenreForUpdateDto genreForUpdate, bool trackChanges)
        {
            var genreEntity = await GetGenreAndCheckIfItExists(genreId, trackChanges);

            _mapper.Map(genreForUpdate, genreEntity);
            await _repository.SaveAsync();
        }

        private async Task<Genre> GetGenreAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var genre = await _repository.Genre.GetGenreAsync(id, trackChanges);
            if (genre is null)
                throw new GenreNotFoundException(id);
            return genre;
        }
    }
}