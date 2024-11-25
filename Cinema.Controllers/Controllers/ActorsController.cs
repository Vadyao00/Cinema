using Cinema.Application.Commands.ActorsCommands;
using Cinema.Application.Queries.ActorsQueries;
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
    [Route("api/actors")]
    [Authorize]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ActorsController : ApiControllerBase
    {
        private readonly ISender _sender;

        public ActorsController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetActors([FromQuery]ActorParameters actorParameters)
        {
            var baseResult = await _sender.Send(new GetActorsQuery(actorParameters, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var (actors, metaData) = baseResult.GetResult<(IEnumerable<ActorDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            ViewBag.ActorParameters = actorParameters;
            ViewBag.MetaData = metaData;

            return Ok(actors);
        }

        [HttpGet("{id:guid}", Name = "ActorById")]
        public async Task<IActionResult> GetActor(Guid id)
        {
            var baseResult = await _sender.Send(new GetActorQuery(id, TrackChanges: false));
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
            var createdActor = await _sender.Send(new CreateActorCommand(actor));

            return CreatedAtRoute("ActorById", new { id = createdActor.ActorId }, createdActor);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteActor(Guid id)
        {
            var baseResult = await _sender.Send(new DeleteActorCommand(id, TrackChanges: false));
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateActor(Guid id, [FromBody] ActorForUpdateDto actor)
        {
            var baseResult = await _sender.Send(new UpdateActorCommand(id,actor, TrackChanges: true));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}