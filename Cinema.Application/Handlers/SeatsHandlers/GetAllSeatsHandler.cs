
using AutoMapper;
using Cinema.Application.Queries.SeatsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.SeatsHandlers
{
    public sealed class GetAllSeatsHandler : IRequestHandler<GetAllSeatsQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetAllSeatsHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetAllSeatsQuery request, CancellationToken cancellationToken)
        {
            var seats = await _repository.Seat.GetAllSeatsWithoutMetaAsync(request.TrackChanges);
            var seatsDto = _mapper.Map<IEnumerable<SeatDto>>(seats);

            return new ApiOkResponse<IEnumerable<SeatDto>>(seatsDto);
        }
    }
}