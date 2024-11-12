using Cinema.Controllers.Extensions;
using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Contracts.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/actors")]
    [Authorize]
    public class ActorsController : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public ActorsController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetActors([FromQuery]ActorParameters actorParameters)
        {
            var baseResult = await _service.Actor.GetAllActorsAsync(actorParameters, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var (actors, metaData) = baseResult.GetResult<(IEnumerable<ActorDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(actors);
        }

        [HttpGet("{id:guid}", Name = "ActorById")]
        public async Task<IActionResult> GetActor(Guid id)
        {
            var baseResult = await _service.Actor.GetActorAsync(id, trackChanges: false);
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var actor = baseResult.GetResult<ActorDto>();

            return Ok(actor);
        }

        [HttpPost(Name = "CreateActor")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateActor([FromBody] ActorForCreationDto actor)
        {
            var createdActor = await _service.Actor.CreateActorAsync(actor);

            return CreatedAtRoute("ActorById", new { id = createdActor.ActorId }, createdActor);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteActor(Guid id)
        {
            var baseResult = await _service.Actor.DeleteActorAsync(id, trackChanges: false);
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateActor(Guid id, [FromBody] ActorForUpdateDto actor)
        {
            var baseResult = await _service.Actor.UpdateActorAsync(id, actor , trackChanges: true);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}