using AutoMapper;
using Cinema.Application.Commands.SeatsCommands;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.SeatsHandlers
{
    public sealed class UpdateSeatHandler : IRequestHandler<UpdateSeatCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UpdateSeatHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(UpdateSeatCommand request, CancellationToken cancellationToken)
        {
            var seatDb = await _repository.Seat.GetSeatAsync(request.Id, request.SeatTrackChanges);
            if (seatDb is null)
                return new SeatNotFoundResponse(request.Id);

            _mapper.Map(request.SeatForUpdate, seatDb);
            await _repository.SaveAsync();

            return new ApiOkResponse<Seat>(seatDb);
        }
    }
}