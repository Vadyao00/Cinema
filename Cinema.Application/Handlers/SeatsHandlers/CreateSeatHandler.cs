using AutoMapper;
using Cinema.Application.Commands.SeatsCommands;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.SeatsHandlers
{
    internal sealed class CreateSeatHandler : IRequestHandler<CreateSeatCommand, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public CreateSeatHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(CreateSeatCommand request, CancellationToken cancellationToken)
        {
            var seatDb = _mapper.Map<Seat>(request.Seat);
            _repository.Seat.CreateSeat(request.EventId, request.ShowtimeId, seatDb);
            await _repository.SaveAsync();

            if (request.ShowtimeId is null && request.EventId is not null)
            {
                var eventDb = await _repository.Event.GetEventAsync((Guid)request.EventId, trackChanges: false);
                if (eventDb is null)
                    return new EventNotFoundResponse((Guid)request.EventId);
                seatDb.Event = eventDb;
                seatDb.Showtime = null;
            }

            if (request.ShowtimeId is not null && request.EventId is null)
            {
                var showtimeDb = await _repository.Showtime.GetShowtimeAsync((Guid)request.ShowtimeId, trackChanges: false);
                if (showtimeDb is null)
                    return new ShowtimeNotFoundResponse((Guid)request.ShowtimeId);
                seatDb.Event = null;
                seatDb.Showtime = showtimeDb;
            }

            var seatToReturn = _mapper.Map<SeatDto>(seatDb);
            return new ApiOkResponse<SeatDto>(seatToReturn);
        }
    }
}