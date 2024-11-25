using AutoMapper;
using Cinema.Application.Queries.SeatsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.SeatsHandlers
{
    public sealed class GetSeatsHandler : IRequestHandler<GetSeatsQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetSeatsHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetSeatsQuery request, CancellationToken cancellationToken)
        {
            if (!request.SeatParameters.ValidSeatNumber)
                return new SeatNumberBadRequestResponse();

            var seatsWithMetaData = await _repository.Seat.GetAllSeatsAsync(request.SeatParameters, request.TrackChanges);
            var seatsDto = _mapper.Map<IEnumerable<SeatDto>>(seatsWithMetaData);

            return new ApiOkResponse<(IEnumerable<SeatDto>, MetaData)>((seatsDto, seatsWithMetaData.MetaData));
        }
    }
}