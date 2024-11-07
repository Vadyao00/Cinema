using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Responses;
using Cinema.LoggerService;
using Contracts.IRepositories;
using Contracts.IServices;

namespace Cinema.Application.Services
{
    public class SeatService : ISeatService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public SeatService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> CreateSeatForShowtimeOrEventAsync(Guid? showtimeId, Guid? eventId, SeatForCreationDto seat, bool trackChanges)
        {
            var seatDb = _mapper.Map<Seat>(seat);

            _repository.Seat.CreateSeat(eventId, showtimeId, seatDb);
            await _repository.SaveAsync();

            if (showtimeId is null && eventId is not null)
            {
                var eventDb = await _repository.Event.GetEventAsync((Guid)eventId, trackChanges: false);
                if (eventDb is null)
                    return new EventNotFoundResponse((Guid)eventId);
                seatDb.Event = eventDb;
                seatDb.Showtime = null;
            }

            if (showtimeId is not null && eventId is null)
            {
                var showtimeDb = await _repository.Showtime.GetShowtimeAsync((Guid)showtimeId, trackChanges: false);
                if (showtimeDb is null)
                    return new ShowtimeNotFoundResponse((Guid)showtimeId);
                seatDb.Event = null;
                seatDb.Showtime = showtimeDb;
            }

            var seatToReturn = _mapper.Map<SeatDto>(seatDb);
            return new ApiOkResponse<SeatDto>(seatToReturn);
        }

        public async Task<ApiBaseResponse> DeleteSeatAsync(Guid Id, bool trackChanges)
        {
            var seatDb = await _repository.Seat.GetSeatAsync(Id, trackChanges);
            if (seatDb is null)
                return new SeatNotFoundResponse(Id);

            _repository.Seat.DeleteSeat(seatDb);
            await _repository.SaveAsync();

            return new ApiOkResponse<Seat>(seatDb);
        }

        public async Task<ApiBaseResponse> GetAllSeatsAsync(bool trackChanges)
        {
            var seats = await _repository.Seat.GetAllSeatsAsync(trackChanges);
            var seatsDto = _mapper.Map<IEnumerable<SeatDto>>(seats);

            return new ApiOkResponse<IEnumerable<SeatDto>>(seatsDto);
        }

        public async Task<ApiBaseResponse> GetSeatAsync(Guid Id, bool trackChanges)
        {
            var seatDb = await _repository.Seat.GetSeatAsync(Id, trackChanges);
            if (seatDb is null)
                return new SeatNotFoundResponse(Id);

            var seatDto = _mapper.Map<SeatDto>(seatDb);
            return new ApiOkResponse<SeatDto>(seatDto);
        }

        public async Task<ApiBaseResponse> UpdateSeatAsync(Guid Id, SeatForUpdateDto seatForUpdate, bool seatTrackChanges)
        {
            var seatDb = await _repository.Seat.GetSeatAsync(Id, seatTrackChanges);
            if (seatDb is null)
                return new SeatNotFoundResponse(Id);

            _mapper.Map(seatForUpdate, seatDb);
            await _repository.SaveAsync();

            return new ApiOkResponse<Seat>(seatDb);
        }
    }
}