using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.RequestFeatures;
using Cinema.Domain.Responses;
using Cinema.LoggerService;
using Contracts.IRepositories;
using Contracts.IServices;

namespace Cinema.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public TicketService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> CreateTicketForSeatAsync(Guid seatId, TicketForCreationDto ticket, bool trackChanges)
        {
            var ticketDb = _mapper.Map<Ticket>(ticket);

            _repository.Ticket.CreateTicketForSeat(seatId, ticketDb);
            await _repository.SaveAsync();

            var seat = await _repository.Seat.GetSeatAsync(seatId, trackChanges);
            if (seat is null)
                return new SeatNotFoundResponse(seatId);
            ticketDb.Seat = seat;

            var ticketToReturn = _mapper.Map<TicketDto>(ticketDb);
            return new ApiOkResponse<TicketDto>(ticketToReturn);
        }

        public async Task<ApiBaseResponse> DeleteTicketAsync(Guid seatId, Guid Id, bool trackChanges)
        {
            var seat = await _repository.Seat.GetSeatAsync(seatId, trackChanges);
            if (seat is null)
                return new SeatNotFoundResponse(seatId);

            var ticket = await _repository.Ticket.GetTicketAsync(Id, trackChanges);
            if (ticket is null)
                return new TicketNotFoundResponse(Id);

            _repository.Ticket.DeleteTicket(ticket);
            await _repository.SaveAsync();

            return new ApiOkResponse<Ticket>(ticket);
        }

        public async Task<ApiBaseResponse> GetAllTicketsForSeatAsync(TicketParameters ticketParameters, Guid seatId, bool trackChanges)
        {
            var seat = await _repository.Seat.GetSeatAsync(seatId, trackChanges);
            if (seat is null)
                return new SeatNotFoundResponse(seatId);

            var ticketsWithMetaData = await _repository.Ticket.GetAllTicketsForSeatAsync(ticketParameters, seatId, trackChanges);
            var ticketsDto = _mapper.Map<IEnumerable<TicketDto>>(ticketsWithMetaData);

            return new ApiOkResponse<(IEnumerable<TicketDto>, MetaData)>((ticketsDto, ticketsWithMetaData.MetaData));
        }

        public async Task<ApiBaseResponse> GetTicketAsync(Guid seatId, Guid Id, bool trackChanges)
        {
            var seat = await _repository.Seat.GetSeatAsync(seatId, trackChanges);
            if (seat is null)
                return new SeatNotFoundResponse(seatId);

            var ticketDb = await _repository.Ticket.GetTicketAsync(Id, trackChanges);
            if (ticketDb is null)
                return new TicketNotFoundResponse(Id);

            var ticketDto = _mapper.Map<TicketDto>(ticketDb);
            return new ApiOkResponse<TicketDto>(ticketDto);
        }

        public async Task<ApiBaseResponse> UpdateTicketAsync(Guid seatId, Guid Id, TicketForUpdateDto ticketForUpdate, bool seatTrackChanges, bool tickTrackChanges)
        {
            var seat = await _repository.Seat.GetSeatAsync(seatId, seatTrackChanges);
            if (seat is null)
                return new SeatNotFoundResponse(seatId);

            var ticketEntity = await _repository.Ticket.GetTicketAsync(Id, tickTrackChanges);
            if (ticketEntity is null)
                return new TicketNotFoundResponse(Id);

            _mapper.Map(ticketForUpdate, ticketEntity);
            await _repository.SaveAsync();

            return new ApiOkResponse<Ticket>(ticketEntity);
        }
    }
}