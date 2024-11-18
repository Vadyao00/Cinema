using AutoMapper;
using Cinema.Application.Commands.MoviesCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.MoviesHandlers
{
    internal sealed class DeleteMovieHandler : IRequestHandler<DeleteMovieCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public DeleteMovieHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            var genre = await _repository.Genre.GetGenreAsync(request.GenreId, request.TrackChanges);
            if (genre is null)
                return new GenreNotFoundResponse(request.GenreId);

            var movieDb = await _repository.Movie.GetMovieAsync(request.GenreId, request.Id, request.TrackChanges);
            if (movieDb is null)
                return new MovieNotFoundResponse(request.Id);

            _repository.Movie.DeleteMovie(movieDb);
            await _repository.SaveAsync();

            return new ApiOkResponse<Movie>(movieDb);
        }
    }
}