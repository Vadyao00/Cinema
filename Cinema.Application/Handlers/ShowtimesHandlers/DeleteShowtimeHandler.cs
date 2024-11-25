using AutoMapper;
using Cinema.Application.Commands.ShowtimesCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ShowtimesHandlers
{
    public sealed class DeleteShowtimeHandler : IRequestHandler<DeleteShowtimeCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public DeleteShowtimeHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(DeleteShowtimeCommand request, CancellationToken cancellationToken)
        {
            var movie = await _repository.Movie.GetMovieAsync(request.MovieId, request.TrackChanges);
            if (movie is null)
                return new MovieNotFoundResponse(request.MovieId);

            var showtimeForMovie = await _repository.Showtime.GetShowtimeForMovieAsync(request.MovieId, request.Id, request.TrackChanges);
            if (showtimeForMovie is null)
                return new ShowtimeNotFoundResponse(request.Id);

            _repository.Showtime.DeleteShowtimeForMovie(showtimeForMovie);
            await _repository.SaveAsync();

            return new ApiOkResponse<Showtime>(showtimeForMovie);
        }
    }
}