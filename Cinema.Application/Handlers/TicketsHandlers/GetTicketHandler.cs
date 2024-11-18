﻿using AutoMapper;
using Cinema.Application.Queries.TicketsQueries;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.Responses;
using Contracts.IRepositories;
using MediatR;

namespace Cinema.Application.Handlers.TicketsHandlers
{
    internal sealed class GetTicketHandler : IRequestHandler<GetTicketQuery, ApiBaseResponse>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public GetTicketHandler(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApiBaseResponse> Handle(GetTicketQuery request, CancellationToken cancellationToken)
        {
            var seat = await _repository.Seat.GetSeatAsync(request.SeatId, request.TrackChanges);
            if (seat is null)
                return new SeatNotFoundResponse(request.SeatId);

            var ticketDb = await _repository.Ticket.GetTicketAsync(request.Id, request.TrackChanges);
            if (ticketDb is null)
                return new TicketNotFoundResponse(request.Id);

            var ticketDto = _mapper.Map<TicketDto>(ticketDb);
            return new ApiOkResponse<TicketDto>(ticketDto);
        }
    }
}