using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Persistence.Extensions;
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
            .Include(a => a.Movies)
            .SingleOrDefaultAsync();

        public async Task<PagedList<Actor>> GetAllActorsAsync(ActorParameters actorParameters, bool trackChanges)
        {
            var actors = await FindAll(trackChanges)
                  .Search(actorParameters.searchName)
                  .Include(x => x.Movies)
                  .Sort(actorParameters.OrderBy)
                  .Skip((actorParameters.PageNumber - 1) * actorParameters.PageSize)
                  .Take(actorParameters.PageSize)
                  .ToListAsync();

            var count = await FindAll(trackChanges).Search(actorParameters.searchName).CountAsync();

            return new PagedList<Actor>(actors, count, actorParameters.PageNumber, actorParameters.PageSize);
        }

        public async Task<IEnumerable<Actor>> GetAllActorsWithoutMetaAsync(bool trackChanges)
        {
            var actors = await FindAll(trackChanges)
                  .ToListAsync();

            return actors;
        }

        public void UpdateActor(Actor actor) => Update(actor);

        public async Task<IEnumerable<Actor>> GetActorsByIdsAsync(Guid[] ids, bool trackChanges) =>
            await FindByCondition(m => ids.Contains(m.ActorId), trackChanges)
                  .ToListAsync();
    }
}