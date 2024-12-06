using AutoMapper;
using Cinema.Application.Commands.MoviesCommands;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.MoviesHandlers
{
    public sealed class CreateMovieHandler : IRequestHandler<CreateMovieCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateMovieHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
        {
            var genre = await _repository.Genre.GetGenreAsync(request.GenreId, request.TrackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(request.GenreId);

            var movieDb = _mapper.Map<Movie>(request.MovieDto);

            movieDb.Actors.Clear();

            var actors = await _repository.Actor.GetActorsByIdsAsync(request.MovieDto.ActorsIds, false);

            if (actors is not null)
                foreach (var actor in actors)
                {
                    _repository.Actor.Attach(actor);
                    movieDb.Actors.Add(actor);
                }

            _repository.Movie.CreateMovieForGenre(request.GenreId, movieDb);
            await _repository.SaveAsync();

            movieDb.Genre = genre;

            var movieToReturn = _mapper.Map<MovieDto>(movieDb);
            return new ApiOkResponse<MovieDto>(movieToReturn);
        }
    }
}