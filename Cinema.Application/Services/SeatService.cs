using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Exceptions;
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

        public async Task<SeatDto> CreateSeatForShowtimeOrEventAsync(Guid? showtimeId, Guid? eventId, SeatForCreationDto seat, bool trackChanges)
        {
            var seatDb = _mapper.Map<Seat>(seat);

            _repository.Seat.CreateSeat(eventId, showtimeId,seatDb);
            await _repository.SaveAsync();

            if (showtimeId is null && eventId is not null)
            {
                var eevent = await GetEventModelAsync((Guid)eventId);
                seatDb.Event = eevent;
                seatDb.Showtime = null;
            }

            if (showtimeId is not null && eventId is null)
            {
                var showtime = await GetShowtimeModelAsync((Guid)showtimeId);
                seatDb.Event = null;
                seatDb.Showtime = showtime;
            }

            var seatToReturn = _mapper.Map<SeatDto>(seatDb);
            return seatToReturn;
        }

        public async Task DeleteSeatAsync(Guid Id, bool trackChanges)
        {
            var seat = await GetSeatForEventOrShowtimeAndCheckIfItExists(Id, trackChanges);

            _repository.Seat.DeleteSeat(seat);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<SeatDto>> GetAllSeatsAsync(bool trackChanges)
        {
            var seats = await _repository.Seat.GetAllSeatsAsync(trackChanges);
            var seatsDto = _mapper.Map<IEnumerable<SeatDto>>(seats);

            return seatsDto;
        }

        public async Task<SeatDto> GetSeatAsync(Guid Id, bool trackChanges)
        {
            var seatDb = await GetSeatForEventOrShowtimeAndCheckIfItExists(Id, trackChanges);

            var seatDto = _mapper.Map<SeatDto>(seatDb);
            return seatDto;
        }

        public async Task UpdateSeatAsync(Guid Id, SeatForUpdateDto seatForUpdate, bool movTrackChanges)
        {
            var seatEntity = await GetSeatForEventOrShowtimeAndCheckIfItExists(Id, movTrackChanges);

            _mapper.Map(seatForUpdate, seatEntity);
            await _repository.SaveAsync();
        }

        private async Task<Seat> GetSeatForEventOrShowtimeAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var seatDb = await _repository.Seat.GetSeatAsync(id, trackChanges);
            if (seatDb is null)
                throw new SeatNotFoundException(id);

            return seatDb;
        }

        private async Task<Showtime> GetShowtimeModelAsync(Guid showtimeId)
        {
            var showtimeDb = await _repository.Showtime.GetShowtimeAsync(showtimeId, trackChanges: false);
            if(showtimeDb is null)
                throw new ShowtimeNotFoundException(showtimeId);

            return showtimeDb;
        }

        private async Task<Event> GetEventModelAsync(Guid eventId)
        {
            var eventDb = await _repository.Event.GetEventAsync(eventId, trackChanges: false);
            if (eventDb is null)
                throw new EventNotFoundException(eventId);

            return eventDb;
        }
    }
}