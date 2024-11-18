using AutoMapper;
using Cinema.Application.Queries.MoviesQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.MoviesHandlers
{
    internal sealed class GetMovieHandler : IRequestHandler<GetMovieQuery, ApiBaseResponse>
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
            var genre = await _repository.Genre.GetGenreAsync(request.GenreId, request.TrackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(request.GenreId);

            var movieDb = await _repository.Movie.GetMovieAsync(request.GenreId, request.Id, request.TrackChanges);
            if (movieDb is null)
                return new MovieNotFoundResponse(request.Id);

            var movieDto = _mapper.Map<MovieDto>(movieDb);
            return new ApiOkResponse<MovieDto>(movieDto);
        }
    }
}