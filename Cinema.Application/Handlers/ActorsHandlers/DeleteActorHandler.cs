using Cinema.Application.Commands.ActorsCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ActorsHandlers
{
    internal sealed class DeleteActorHandler : IRequestHandler<DeleteActorCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        public DeleteActorHandler(IRepositoryManager repository) => _repository = repository;

        public async Task<ApiBaseResponse> Handle(DeleteActorCommand request, CancellationToken cancellationToken)
        {
            var actor = await _repository.Actor.GetActorAsync(request.Id, request.TrackChanges);

            if (actor is null)
                return new ActorNotFoundResponse(request.Id);

            _repository.Actor.DeleteActor(actor);
            await _repository.SaveAsync();

            return new ApiOkResponse<Actor>(actor);
        }
    }
}