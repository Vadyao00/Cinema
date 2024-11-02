using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/seats/{seatId}/tickets")]
    public class TicketsControllers : Controller
    {
        private readonly IServiceManager _service;

        public TicketsControllers(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetTicketsForSeat(Guid seatId)
        {
            var tickets = await _service.Ticket.GetAllTicketsForSeatAsync(seatId, trackChanges: false);

            return Ok(tickets);
        }

        [HttpGet("{id:guid}", Name = "GetTicketById")]
        public async Task<IActionResult> GetTicketForSeat(Guid seatId, Guid id)
        {
            var ticket = await _service.Ticket.GetTicketAsync(seatId, id, trackChanges: false);

            return Ok(ticket);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTicketForSeat(Guid seatId, [FromBody] TicketForCreationDto ticket)
        {
            var createdTicket = await _service.Ticket.CreateTicketForSeatAsync(seatId, ticket, trackChanges: false);

            return CreatedAtRoute("GetTicketById", new { seatId = seatId, id = createdTicket.TicketId }, createdTicket);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTicket(Guid id)
        {
            await _service.Ticket.DeleteTicketAsync(id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTicket(Guid seatId, Guid id, [FromBody] TicketForUpdateDto ticket)
        {
            await _service.Ticket.UpdateTicketAsync(seatId, id, ticket, seatTrackChanges: false, tickTrackChanges: true);

            return NoContent();
        }
    }
}