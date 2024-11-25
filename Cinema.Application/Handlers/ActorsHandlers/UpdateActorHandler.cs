using AutoMapper;
using Cinema.Application.Commands.ActorsCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ActorsHandlers
{
    public sealed class UpdateActorHandler : IRequestHandler<UpdateActorCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        public UpdateActorHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(UpdateActorCommand request, CancellationToken cancellationToken)
        {
            var actor = await _repository.Actor.GetActorAsync(request.Id, request.TrackChanges);

            if (actor is null)
                return new ActorNotFoundResponse(request.Id);

            _mapper.Map(request.ActorForUpdateDto, actor);
            await _repository.SaveAsync();

            return new ApiOkResponse<Actor>(actor);
        }
    }
}