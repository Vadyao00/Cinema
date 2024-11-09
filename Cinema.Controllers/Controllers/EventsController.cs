using Cinema.Controllers.Extensions;
using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public EventsController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetEvents([FromQuery]EventParameters eventParameters)
        {
            var baseResult = await _service.Event.GetAllEventsAsync(eventParameters, trackChanges: false);

            var (events, metaData) = baseResult.GetResult<(IEnumerable<EventDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(events);
        }

        [HttpGet("{id:guid}", Name = "EventById")]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            var baseResult = await _service.Event.GetEventAsync(id, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var eevent = baseResult.GetResult<EventDto>();

            return Ok(eevent);
        }

        [HttpPost(Name = "CreateEvent")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEvent([FromBody] EventForCreationDto eevent)
        {
            var createdEvent = await _service.Event.CreateEventAsync(eevent);

            return CreatedAtRoute("EventById", new { id = createdEvent.EventId }, createdEvent);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var baseResult = await _service.Event.DeleteEventAsync(id, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventForUpdateDto actor)
        {
            var baseResult = await _service.Event.UpdateEventAsync(id, actor, trackChanges: true);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}