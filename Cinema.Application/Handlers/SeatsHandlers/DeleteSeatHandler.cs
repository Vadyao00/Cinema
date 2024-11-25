using AutoMapper;
using Cinema.Application.Commands.SeatsCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.SeatsHandlers
{
    public sealed class DeleteSeatHandler : IRequestHandler<DeleteSeatCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public DeleteSeatHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(DeleteSeatCommand request, CancellationToken cancellationToken)
        {
            var seatDb = await _repository.Seat.GetSeatAsync(request.Id, request.TrackChanges);
            if (seatDb is null)
                return new SeatNotFoundResponse(request.Id);

            _repository.Seat.DeleteSeat(seatDb);
            await _repository.SaveAsync();

            return new ApiOkResponse<Seat>(seatDb);
        }
    }
}