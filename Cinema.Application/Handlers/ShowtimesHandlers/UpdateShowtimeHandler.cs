using AutoMapper;
using Cinema.Application.Commands.ShowtimesCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ShowtimesHandlers
{
    public sealed class UpdateShowtimeHandler : IRequestHandler<UpdateShowtimeCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateShowtimeHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(UpdateShowtimeCommand request, CancellationToken cancellationToken)
        {
            var movie = await _repository.Movie.GetMovieAsync(request.MovieId, request.MovTrackChanges);
            if (movie is null)
                return new MovieNotFoundResponse(request.MovieId);

            var showtimeEntity = await _repository.Showtime.GetShowtimeForMovieAsync(request.MovieId, request.Id, request.ShwTrackChanges);
            if (showtimeEntity is null)
                return new ShowtimeNotFoundResponse(request.Id);

            _mapper.Map(request.ShowtimeForUpdate, showtimeEntity);
            await _repository.SaveAsync();

            return new ApiOkResponse<Showtime>(showtimeEntity);
        }
    }
}