using Cinema.Domain.DataTransferObjects;

namespace Contracts.IServices
{
    public interface IActorService
    {
        Task<IEnumerable<ActorDto>> GetAllActorsAsync(bool trackChanges);
        Task<ActorDto> GetActorAsync(Guid actorId, bool trackChanges);
        Task<ActorDto> CreateActorAsync(ActorForCreationDto actor);
        Task<IEnumerable<ActorDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        Task DeleteActorAsync(Guid actorId, bool trackChanges);
        Task UpdateActorAsync(Guid actorId, ActorForUpdateDto actorForUpdate, bool trackChanges);
    }
}