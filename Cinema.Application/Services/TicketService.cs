using AutoMapper;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Entities;
using Cinema.Domain.Exceptions;
using Cinema.LoggerService;
using Contracts.IRepositories;
using Contracts.IServices;
using Microsoft.Extensions.Logging;

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

        public async Task<TicketDto> CreateTicketForSeatAsync(Guid seatId, TicketForCreationDto ticket, bool trackChanges)
        {
            var ticketDb = _mapper.Map<Ticket>(ticket);

            _repository.Ticket.CreateTicketForSeat(seatId, ticketDb);
            await _repository.SaveAsync();

            var seat = await GetSeatModelAsync(seatId, trackChanges);
            ticketDb.Seat = seat;

            var ticketToReturn = _mapper.Map<TicketDto>(ticketDb);
            return ticketToReturn;
        }

        public async Task DeleteTicketAsync(Guid Id, bool trackChanges)
        {
            var ticket = await GetTicketForSeatAndCheckIfItExists(Id, trackChanges);

            _repository.Ticket.DeleteTicket(ticket);
            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<TicketDto>> GetAllTicketsForSeatAsync(Guid seatId, bool trackChanges)
        {
            var tickets = await _repository.Ticket.GetAllTicketsForSeatAsync(seatId, trackChanges);
            var ticketsDto = _mapper.Map<IEnumerable<TicketDto>>(tickets);

            return ticketsDto;
        }

        public async Task<TicketDto> GetTicketAsync(Guid seatId, Guid Id, bool trackChanges)
        {
            var ticketDb = await GetTicketForSeatAndCheckIfItExists(Id, trackChanges);

            var ticketDto = _mapper.Map<TicketDto>(ticketDb);
            return ticketDto;
        }

        public async Task UpdateTicketAsync(Guid seatId, Guid Id, TicketForUpdateDto ticketForUpdate, bool seatTrackChanges, bool tickTrackChanges)
        {
            await CheckIfSeatExists(seatId, seatTrackChanges);

            var movieEntity = await GetTicketForSeatAndCheckIfItExists(Id, tickTrackChanges);

            _mapper.Map(ticketForUpdate, movieEntity);
            await _repository.SaveAsync();
        }

        private async Task CheckIfSeatExists(Guid seatId, bool trackChanges)
        {
            var seat = await _repository.Seat.GetSeatAsync(seatId, trackChanges);
            if (seat is null)
                throw new SeatNotFoundException(seatId);
        }

        private async Task<Seat> GetSeatModelAsync(Guid seatId, bool trackChanges)
        {
            var seat = await _repository.Seat.GetSeatAsync(seatId, trackChanges);
            if(seat is null)
                throw new SeatNotFoundException(seatId);

            return seat;
        }

        private async Task<Ticket> GetTicketForSeatAndCheckIfItExists(Guid id, bool trackChanges)
        {
            var ticketDb = await _repository.Ticket.GetTicketAsync(id, trackChanges);
            if (ticketDb is null)
                throw new TicketNotFoundException(id);

            return ticketDb;
        }
    }
}
