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

        public async Task<ApiBaseResponse> DeleteGenreAsync(Guid genreId, bool trackChanges)
        {
            var genre = await _repository.Genre.GetGenreAsync(genreId, trackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(genreId);

            _repository.Genre.DeleteGenre(genre);
            await _repository.SaveAsync();

            return new ApiOkResponse<Genre>(genre);
        }

        public async Task<ApiBaseResponse> GetAllGenresAsync(GenreParameters genreParameters, bool trackChanges)
        {
            var genresWithMetaData = await _repository.Genre.GetAllGenresAsync(genreParameters, trackChanges);
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genresWithMetaData);

            return new ApiOkResponse<(IEnumerable<GenreDto>, MetaData)>((genresDto, genresWithMetaData.MetaData));
        }

        public async Task<ApiBaseResponse> GetGenreAsync(Guid genreId, bool trackChanges)
        {
            var genre = await _repository.Genre.GetGenreAsync(genreId, trackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(genreId);

            var genreDto = _mapper.Map<GenreDto>(genre);

            return new ApiOkResponse<GenreDto>(genreDto);
        }

        public async Task<ApiBaseResponse> UpdateGenreAsync(Guid genreId, GenreForUpdateDto genreForUpdate, bool trackChanges)
        {
            var genre = await _repository.Genre.GetGenreAsync(genreId, trackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(genreId);

            _mapper.Map(genreForUpdate, genre);
            await _repository.SaveAsync();

            return new ApiOkResponse<Genre>(genre);
        }
    }
}