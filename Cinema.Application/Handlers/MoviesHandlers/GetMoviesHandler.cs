using AutoMapper;
using Cinema.Application.Queries.MoviesQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.MoviesHandlers
{
    public sealed class GetMoviesHandler : IRequestHandler<GetMoviesQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetMoviesHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            if (!request.MovieParameters.ValidAgeRestriction)
                return new AgeRestrictionBadRequestResponse();

            var moviesWithMetaData = await _repository.Movie.GetAllMoviesAsync(request.MovieParameters, request.TrackChanges);
            var moviesDto = _mapper.Map<IEnumerable<MovieDto>>(moviesWithMetaData);

            return new ApiOkResponse<(IEnumerable<MovieDto>, MetaData)>((moviesDto, moviesWithMetaData.MetaData));
        }
    }
}