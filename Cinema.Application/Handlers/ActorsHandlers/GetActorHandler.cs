using AutoMapper;
using Cinema.Application.Queries.ActorsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ActorsHandlers
{
    public sealed class GetActorHandler : IRequestHandler<GetActorQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetActorHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetActorQuery request, CancellationToken cancellationToken)
        {
            var actor = await _repository.Actor.GetActorAsync(request.Id, request.TrackChanges);

            if (actor is null)
                return new ActorNotFoundResponse(request.Id);

            var actorDto = _mapper.Map<ActorDto>(actor);

            return new ApiOkResponse<ActorDto>(actorDto);
        }
    }
}