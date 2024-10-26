using Cinema.Domain.Entities;
using Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Persistence.Repositories
{
    public class ActorRepository(CinemaContext dbContext) : RepositoryBase<Actor>(dbContext), IActorRepository
    {
        public void CreateActor(Actor actor) => Create(actor);

        public void DeleteActor(Actor actor) => Delete(actor);

        public async Task<Actor> GetActorAsync(Guid id, bool trackChanges) =>
            await FindByCondition(a => a.ActorId.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Actor>> GetAllActorsAsync(bool trackChanges) =>
            await FindAll(trackChanges)
                  .OrderBy(a => a.Name)
                  .ToListAsync();

        public async Task<IEnumerable<Actor>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges) =>
            await FindByCondition(x => ids.Contains(x.ActorId), trackChanges)
                  .ToListAsync();
    }
}