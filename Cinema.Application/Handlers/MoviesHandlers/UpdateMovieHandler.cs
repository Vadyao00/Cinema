using AutoMapper;
using Cinema.Application.Commands.MoviesCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.MoviesHandlers
{
    public sealed class UpdateMovieHandler : IRequestHandler<UpdateMovieCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateMovieHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var movieDb = await _repository.Movie.GetMovieAsync(request.Id, request.MovTrackChanges);
            if (movieDb is null)
                return new MovieNotFoundResponse(request.Id);

            _mapper.Map(request.MovieForUpdate, movieDb);
            await _repository.SaveAsync();

            return new ApiOkResponse<Movie>(movieDb);
        }
    }
}