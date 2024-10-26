using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Exceptions;
using Cinema.LoggerService;
using Contracts.IRepositories;
using Contracts.IServices;

namespace Cinema.Application.Services
{
    public sealed class ActorService : IActorService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ActorService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActorDto> CreateActorAsync(ActorForCreationDto actor)
        {
            var actorEntity = _mapper.Map<Actor>(actor);

            _repository.Actor.CreateActor(actorEntity);
            await _repository.SaveAsync();

            var actorToReturn = _mapper.Map<ActorDto>(actorEntity);

            return actorToReturn;
        }

        public async Task DeleteActorAsync(Guid actorId, bool trackChanges)
        {
            var actor = await GetActorAndCheckIfItExists(actorId, trackChanges);

            _repository.Actor.DeleteActor(actor);
            await _repository.SaveAsync();
        }

        public async Task<ActorDto> GetActorAsync(Guid actorId, bool trackChanges)
        {
            var actor = await GetActorAndCheckIfItExists(actorId, trackChanges);

            var actorDto = _mapper.Map<ActorDto>(actor);

            return actorDto;
        }

        public async Task<IEnumerable<ActorDto>> GetAllActorsAsync(bool trackChanges)
        {
            var companies = await _repository.Actor.GetAllActorsAsync(trackChanges);
            var companiesDto = _mapper.Map<IEnumerable<ActorDto>>(companies);

            return companiesDto;
        }

        public async Task<IEnumerable<ActorDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametrBadRequestException();

            var actorEntities = await _repository.Actor.GetByIdsAsync(ids, trackChanges);
            if(ids.Count() != actorEntities.Count())
                throw new IdParametrBadRequestException();

            var actorsToReturn = _mapper.Map<IEnumerable<ActorDto>>(actorEntities);

            return actorsToReturn;
        }

        public async Task UpdateActorAsync(Guid actorId, ActorForUpdateDto actorForUpdate, bool trackChanges)
        {
            var actorEntity = await GetActorAndCheckIfItExists(actorId, trackChanges);

            _mapper.Map(actorForUpdate, actorEntity);
            await _repository.SaveAsync();
        }

        private async Task<Actor> GetActorAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var actor = await _repository.Actor.GetActorAsync(id, trackChanges);
            if (actor is null)
                throw new ActorNotFoundException(id);
            return actor;
        }
    }
}