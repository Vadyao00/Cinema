using AutoMapper;
using Cinema.Application.Queries.MoviesQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.MoviesHandlers
{
    public sealed class GetMovieHandler : IRequestHandler<GetMovieQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetMovieHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetMovieQuery request, CancellationToken cancellationToken)
        {
            var movieDb = await _repository.Movie.GetMovieAsync(request.Id, request.TrackChanges);
            if (movieDb is null)
                return new MovieNotFoundResponse(request.Id);

            var movieDto = _mapper.Map<MovieDto>(movieDb);
            return new ApiOkResponse<MovieDto>(movieDto);
        }
    }
}