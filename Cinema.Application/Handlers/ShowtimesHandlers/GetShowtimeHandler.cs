using AutoMapper;
using Cinema.Application.Queries.ShowtimesQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ShowtimesHandlers
{
    internal sealed class GetShowtimeHandler : IRequestHandler<GetShowtimeQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetShowtimeHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetShowtimeQuery request, CancellationToken cancellationToken)
        {
            var movie = await _repository.Movie.GetMovieAsync(request.GenreId, request.MovieId, request.TrackChanges);
            if (movie is null)
                return new MovieNotFoundResponse(request.MovieId);

            var showtimeDb = await _repository.Showtime.GetShowtimeForMovieAsync(request.MovieId, request.Id, request.TrackChanges);
            if (showtimeDb is null)
                return new ShowtimeNotFoundResponse(request.Id);

            var showtimeDto = _mapper.Map<ShowtimeDto>(showtimeDb);
            return
                new ApiOkResponse<ShowtimeDto>(showtimeDto);
        }
    }
}