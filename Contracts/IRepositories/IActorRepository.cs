using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;

namespace Contracts.IRepositories
{
    public interface IActorRepository
    {
        Task<PagedList<Actor>> GetAllActorsAsync(ActorParameters actorParameters, bool trackChanges);
        Task<Actor> GetActorAsync(Guid id, bool trackChanges);
        Task<IEnumerable<Actor>> GetAllActorsWithoutMetaAsync(bool trackChanges);
        void CreateActor(Actor actor);
        void DeleteActor(Actor actor);
        void UpdateActor(Actor actor);
        Task<IEnumerable<Actor>> GetActorsByIdsAsync(Guid[] ids, bool trackChanges);
    }
}