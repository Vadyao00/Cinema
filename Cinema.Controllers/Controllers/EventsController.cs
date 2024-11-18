using Cinema.Application.Commands.EventsCommands;
using Cinema.Application.Queries.EventsQueries;
using Cinema.Controllers.Extensions;
using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Contracts.IServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/events")]
    [Authorize]
    public class EventsController : ApiControllerBase
    {
        private readonly ISender _sender;

        public EventsController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery]EventParameters eventParameters)
        {
            var baseResult = await _sender.Send(new GetEventsQuery(eventParameters, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var (events, metaData) = baseResult.GetResult<(IEnumerable<EventDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(events);
        }

        [HttpGet("{id:guid}", Name = "EventById")]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            var baseResult = await _sender.Send(new GetEventQuery(id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var eevent = baseResult.GetResult<EventDto>();

            return Ok(eevent);
        }

        [HttpPost(Name = "CreateEvent")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateEvent([FromBody] EventForCreationDto eevent)
        {
            var createdEvent = await _sender.Send(new CreateEventCommand(eevent));

            return CreatedAtRoute("EventById", new { id = createdEvent.EventId }, createdEvent);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var baseResult = await _sender.Send(new DeleteEventCommand(id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventForUpdateDto eevent)
        {
            var baseResult = await _sender.Send(new UpdateEventCommand(id, eevent, TrackChanges: true));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}