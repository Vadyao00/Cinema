using Cinema.Domain.Entities;

namespace Contracts.IRepositories
{
    public interface IActorRepository
    {
        Task<IEnumerable<Actor>> GetAllActorsAsync(bool trackChanges);
        Task<Actor> GetActorAsync(Guid id, bool trackChanges);
        void CreateActor(Actor actor);
        Task<IEnumerable<Actor>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteActor(Actor actor);
    }
}
