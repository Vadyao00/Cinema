using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : Controller
    {
        private readonly IServiceManager _service;

        public ActorsController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetActors()
        {
            var actors = await _service.Actor.GetAllActorsAsync(trackChanges: false);

            return Ok(actors);
        }

        [HttpGet("{id:guid}", Name = "ActorById")]
        public async Task<IActionResult> GetActor(Guid id)
        {
            var actor = await _service.Actor.GetActorAsync(id, trackChanges: false);

            return Ok(actor);
        }

        [HttpPost(Name = "CreateActor")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateActor([FromBody] ActorForCreationDto actor)
        {
            var createdActor = await _service.Actor.CreateActorAsync(actor);

            return CreatedAtRoute("ActorById", new { id = createdActor.ActorId }, createdActor);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteActor(Guid id)
        {
            await _service.Actor.DeleteActorAsync(id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateActor(Guid id, [FromBody] ActorForUpdateDto actor)
        {
            await _service.Actor.UpdateActorAsync(id, actor , trackChanges: true);

            return NoContent();
        }
    }
}