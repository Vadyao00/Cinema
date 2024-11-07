using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;

namespace Contracts.IServices
{
    public interface IActorService
    {
        Task<ApiBaseResponse> GetAllActorsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetActorAsync(Guid actorId, bool trackChanges);
        Task<ActorDto> CreateActorAsync(ActorForCreationDto actor);
        Task<ApiBaseResponse> DeleteActorAsync(Guid actorId, bool trackChanges);
        Task<ApiBaseResponse> UpdateActorAsync(Guid actorId, ActorForUpdateDto actorForUpdate, bool trackChanges);
    }
}