using AutoMapper;
using Cinema.Application.Queries.ShowtimesQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ShowtimesHandlers
{
    public sealed class GetAllShowtimesHandler : IRequestHandler<GetAllShowtimesQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetAllShowtimesHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetAllShowtimesQuery request, CancellationToken cancellationToken)
        {
            var showtimes = await _repository.Showtime.GetAllShowtimesWithoutMetaAsync(request.TrackChanges);
            var showtimesDto = _mapper.Map<IEnumerable<ShowtimeDto>>(showtimes);

            return new ApiOkResponse<IEnumerable<ShowtimeDto>>(showtimesDto);
        }
    }
}