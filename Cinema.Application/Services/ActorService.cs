﻿using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Exceptions;
using Cinema.Domain.Responses;
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

        public async Task<ApiBaseResponse> DeleteActorAsync(Guid actorId, bool trackChanges)
        {
            var actor = await _repository.Actor.GetActorAsync(actorId, trackChanges);

            if (actor is null)
                return new ActorNotFoundResponse(actorId);

            _repository.Actor.DeleteActor(actor);
            await _repository.SaveAsync();

            return new ApiOkResponse<Actor>(actor);
        }

        public async Task<ApiBaseResponse> GetActorAsync(Guid actorId, bool trackChanges)
        {
            var actor = await _repository.Actor.GetActorAsync(actorId, trackChanges);

            if(actor is null)
                return new ActorNotFoundResponse(actorId);

            var actorDto = _mapper.Map<ActorDto>(actor);

            return new ApiOkResponse<ActorDto>(actorDto);
        }

        public async Task<ApiBaseResponse> GetAllActorsAsync(bool trackChanges)
        {
            var actors = await _repository.Actor.GetAllActorsAsync(trackChanges);
            var actorsDto = _mapper.Map<IEnumerable<ActorDto>>(actors);

            return new ApiOkResponse<IEnumerable<ActorDto>>(actorsDto);
        }

        public async Task<ApiBaseResponse> UpdateActorAsync(Guid actorId, ActorForUpdateDto actorForUpdate, bool trackChanges)
        {
            var actor = await _repository.Actor.GetActorAsync(actorId, trackChanges);

            if (actor is null)
                return new ActorNotFoundResponse(actorId);

            _mapper.Map(actorForUpdate, actor);
            await _repository.SaveAsync();

            return new ApiOkResponse<Actor>(actor);
        }
    }
}