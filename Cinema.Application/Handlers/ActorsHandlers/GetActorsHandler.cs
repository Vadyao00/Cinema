using AutoMapper;
using Cinema.Application.Queries.ActorsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ActorsHandlers
{
    public sealed class GetActorsHandler : IRequestHandler<GetActorsQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetActorsHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetActorsQuery request, CancellationToken cancellationToken)
        {
            var actorsWithMetaData = await _repository.Actor.GetAllActorsAsync(request.ActorParameters, request.TrackChanges);
            var actorsDto = _mapper.Map<IEnumerable<ActorDto>>(actorsWithMetaData);

            return new ApiOkResponse<(IEnumerable<ActorDto>, MetaData)>((actorsDto, actorsWithMetaData.MetaData));
        }
    }
}