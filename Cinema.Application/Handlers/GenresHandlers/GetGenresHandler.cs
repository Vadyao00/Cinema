using AutoMapper;
using Cinema.Application.Queries.GenresQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.GenresHandlers
{
    public sealed class GetGenresHandler : IRequestHandler<GetGenresQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetGenresHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            var genresWithMetaData = await _repository.Genre.GetAllGenresAsync(request.GenreParameters, request.TrackChanges);
            var genresDto = _mapper.Map<IEnumerable<GenreDto>>(genresWithMetaData);

            return new ApiOkResponse<(IEnumerable<GenreDto>, MetaData)>((genresDto, genresWithMetaData.MetaData));
        }
    }
}