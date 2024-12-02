using AutoMapper;
using Cinema.Application.Queries.ActorsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ActorsHandlers
{
    public sealed class GetAllActorsHandler : IRequestHandler<GetAllActorsQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetAllActorsHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetAllActorsQuery request, CancellationToken cancellationToken)
        {
            var actors = await _repository.Actor.GetAllActorsWithoutMetaAsync(request.TrackChanges);
            var actorsDto = _mapper.Map<IEnumerable<ActorDto>>(actors);

            return new ApiOkResponse<IEnumerable<ActorDto>>(actorsDto);
        }
    }
}