using AutoMapper;
using Cinema.Application.Queries.SeatsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.SeatsHandlers
{
    public sealed class GetSeatHandler : IRequestHandler<GetSeatQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetSeatHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetSeatQuery request, CancellationToken cancellationToken)
        {
            var seatDb = await _repository.Seat.GetSeatAsync(request.Id, request.TrackChanges);
            if (seatDb is null)
                return new SeatNotFoundResponse(request.Id);

            var seatDto = _mapper.Map<SeatDto>(seatDb);
            return new ApiOkResponse<SeatDto>(seatDto);
        }
    }
}