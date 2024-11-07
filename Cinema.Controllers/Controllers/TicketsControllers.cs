using Cinema.Controllers.Extensions;
using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/seats/{seatId}/tickets")]
    public class TicketsControllers : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public TicketsControllers(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetTicketsForSeat(Guid seatId)
        {
            var baseResult = await _service.Ticket.GetAllTicketsForSeatAsync(seatId, trackChanges: false);
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var tickets = baseResult.GetResult<IEnumerable<TicketDto>>();

            return Ok(tickets);
        }

        [HttpGet("{id:guid}", Name = "GetTicketById")]
        public async Task<IActionResult> GetTicketForSeat(Guid seatId, Guid id)
        {
            var baseResult = await _service.Ticket.GetTicketAsync(seatId, id, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var ticket = baseResult.GetResult<TicketDto>();

            return Ok(ticket);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTicketForSeat(Guid seatId, [FromBody] TicketForCreationDto ticket)
        {
            var baseResult = await _service.Ticket.CreateTicketForSeatAsync(seatId, ticket, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdTicket = baseResult.GetResult<TicketDto>();

            return CreatedAtRoute("GetTicketById", new { seatId = seatId, id = createdTicket.TicketId }, createdTicket);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTicket(Guid seatId, Guid id)
        {
            var baseResult = await _service.Ticket.DeleteTicketAsync(seatId, id, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTicket(Guid seatId, Guid id, [FromBody] TicketForUpdateDto ticket)
        {
            var baseResult = await _service.Ticket.UpdateTicketAsync(seatId, id, ticket, seatTrackChanges: false, tickTrackChanges: true);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}