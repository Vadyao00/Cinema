using AutoMapper;
using Cinema.Application.Queries.MoviesQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.MoviesHandlers
{
    internal sealed class GetMoviesHandler : IRequestHandler<GetMoviesQuery, ApiBaseResponse>
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

            var genre = await _repository.Genre.GetGenreAsync(request.GenreId, request.TrackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(request.GenreId);

            var moviesWithMetaData = await _repository.Movie.GetAllMoviesForGenreAsync(request.MovieParameters, request.GenreId, request.TrackChanges);
            var moviesDto = _mapper.Map<IEnumerable<MovieDto>>(moviesWithMetaData);

            return new ApiOkResponse<(IEnumerable<MovieDto>, MetaData)>((moviesDto, moviesWithMetaData.MetaData));
        }
    }
}