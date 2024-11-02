using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : Controller
    {
        private readonly IServiceManager _service;

        public EventsController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _service.Event.GetAllEventsAsync(trackChanges: false);

            return Ok(events);
        }

        [HttpGet("{id:guid}", Name = "EventById")]
        public async Task<IActionResult> GetEvent(Guid id)
        {
            var eevent = await _service.Event.GetEventAsync(id, trackChanges: false);

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
            await _service.Event.DeleteEventAsync(id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] EventForUpdateDto actor)
        {
            await _service.Event.UpdateEventAsync(id, actor, trackChanges: true);

            return NoContent();
        }
    }
}