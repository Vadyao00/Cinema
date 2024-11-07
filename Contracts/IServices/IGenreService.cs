using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;

namespace Contracts.IServices
{
    public interface IGenreService
    {
        Task<ApiBaseResponse> GetAllGenresAsync(bool trackChanges);
        Task<ApiBaseResponse> GetGenreAsync(Guid genreId, bool trackChanges);
        Task<GenreDto> CreateGenreAsync(GenreForCreationDto genre);
        Task<ApiBaseResponse> DeleteGenreAsync(Guid genreId, bool trackChanges);
        Task<ApiBaseResponse> UpdateGenreAsync(Guid genreId, GenreForUpdateDto genreForUpdate, bool trackChanges);
    }
}
