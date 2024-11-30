using Cinema.Application.Commands.ShowtimesCommands;
using Cinema.Application.Queries.ShowtimesQueries;
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
    [Route("api/showtimes")]
    [Authorize]
    public class ShowtimesController : ApiControllerBase
    {
        private readonly ISender _sender;

        public ShowtimesController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetShowtimes([FromQuery]ShowtimeParameters showtimeParameters)
        {
            var baseResult = await _sender.Send(new GetShowtimesQuery(showtimeParameters, TrackChanges: false));
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var (showtimes, metaData) = baseResult.GetResult<(IEnumerable<ShowtimeDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(showtimes);
        }

        [HttpGet("{id:guid}", Name = "GetShowtimeById")]
        public async Task<IActionResult> GetShowtimeForMovie(Guid id)
        {
            var baseResult = await _sender.Send(new GetShowtimeQuery(id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var showtime = baseResult.GetResult<ShowtimeDto>();

            return Ok(showtime);
        }

        [HttpPost("{movieId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateShowtimeForMovie(Guid movieId, [FromBody] ShowtimeForCreationDto showtime)
        {
            var baseResult = await _sender.Send(new CreateShowtimeCommand(movieId, showtime, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdShowtime = baseResult.GetResult<ShowtimeDto>();

            return CreatedAtRoute("GetShowtimeById", new { id = createdShowtime.ShowtimeId }, createdShowtime);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteShowtime(Guid id)
        {
            var baseResult = await _sender.Send(new DeleteShowtimeCommand(id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateShowtimeForMovie(Guid id, [FromBody] ShowtimeForUpdateDto showtime)
        {
            var baseResult = await _sender.Send(new UpdateShowtimeCommand(id, showtime, MovTrackChanges: false, ShwTrackChanges: true));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}