using Cinema.Application.Commands.TicketsCommands;
using Cinema.Application.Queries.TicketsQueries;
using Cinema.Controllers.Extensions;
using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    [Authorize]
    public class TicketsControllers : ApiControllerBase
    {
        private readonly ISender _sender;

        public TicketsControllers(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetTickets([FromQuery]TicketParameters ticketParameters)
        {
            var baseResult = await _sender.Send(new GetTicketsQuery(ticketParameters, TrackChanges: false));
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var (tickets, metaData) = baseResult.GetResult<(IEnumerable<TicketDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(tickets);
        }

        [HttpGet("{id:guid}", Name = "GetTicketById")]
        public async Task<IActionResult> GetTicket(Guid id)
        {
            var baseResult = await _sender.Send(new GetTicketQuery(id , TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var ticket = baseResult.GetResult<TicketDto>();

            return Ok(ticket);
        }

        [HttpPost("{seatId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateTicketForSeat(Guid seatId, [FromBody] TicketForCreationDto ticket)
        {
            var baseResult = await _sender.Send(new CreateTicketCommand(seatId, ticket, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdTicket = baseResult.GetResult<TicketDto>();

            return CreatedAtRoute("GetTicketById", new { id = createdTicket.TicketId }, createdTicket);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            var baseResult = await _sender.Send(new DeleteTicketCommand(id ,TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateTicket(Guid id, [FromBody] TicketForUpdateDto ticket)
        {
            var baseResult = await _sender.Send(new UpdateTicketCommand(id, ticket, SeatTrackChanges: false, TickTrackChanges: true));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}