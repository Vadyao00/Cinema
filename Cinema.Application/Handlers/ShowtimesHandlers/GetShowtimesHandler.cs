using AutoMapper;
using Cinema.Application.Queries.ShowtimesQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.ShowtimesHandlers
{
    public sealed class GetShowtimesHandler : IRequestHandler<GetShowtimesQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetShowtimesHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetShowtimesQuery request, CancellationToken cancellationToken)
        {
            if (!request.ShowtimeParameters.ValidTicketPriceRange)
                return new MaxTicketPriceBadRequestPesponse();
            if (!request.ShowtimeParameters.ValidTimeRange)
                return new TimeRangeBadRequestResponse();

            var showtimesWithMetaData = await _repository.Showtime.GetAllShowtimesAsync(request.ShowtimeParameters, request.TrackChanges);
            var showtimesDto = _mapper.Map<IEnumerable<ShowtimeDto>>(showtimesWithMetaData);

            return new ApiOkResponse<(IEnumerable<ShowtimeDto>, MetaData)>((showtimesDto, showtimesWithMetaData.MetaData));
        }
    }
}