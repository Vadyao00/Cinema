using AutoMapper;
using Cinema.Application.Queries.MoviesQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.MoviesHandlers
{
    public sealed class GetAllMoviesHandler : IRequestHandler<GetAllMoviesQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetAllMoviesHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _repository.Movie.GetAllWithoutPaginationMoviesAsync(request.TrackChanges);
            var moviesDto = _mapper.Map<IEnumerable<MovieDto>>(movies);

            return new ApiOkResponse<IEnumerable<MovieDto>>(moviesDto);
        }
    }
}