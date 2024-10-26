using Cinema.Domain.DataTransferObjects;

namespace Contracts.IServices
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDto>> GetAllGenresAsync(bool trackChanges);
        Task<GenreDto> GetGenreAsync(Guid genreId, bool trackChanges);
        Task<GenreDto> CreateGenreAsync(GenreForCreationDto genre);
        Task<IEnumerable<GenreDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        Task DeleteGenreAsync(Guid genreId, bool trackChanges);
        Task UpdateGenreAsync(Guid genreId, GenreForUpdateDto genreForUpdate, bool trackChanges);
    }
}
