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
    [Route("api/genres/{genreId:guid}/movies/{movieId:guid}/showtimes")]
    [Authorize]
    public class ShowtimesController : ApiControllerBase
    {
        private readonly ISender _sender;

        public ShowtimesController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetShowtimesForMovie([FromQuery]ShowtimeParameters showtimeParameters, Guid genreId, Guid movieId)
        {
            var baseResult = await _sender.Send(new GetShowtimesQuery(showtimeParameters, genreId, movieId, TrackChanges: false));
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var (showtimes, metaData) = baseResult.GetResult<(IEnumerable<ShowtimeDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(showtimes);
        }

        [HttpGet("{id:guid}", Name = "GetShowtimeById")]
        public async Task<IActionResult> GetShowtimeForMovie(Guid genreId, Guid movieId, Guid id)
        {
            var baseResult = await _sender.Send(new GetShowtimeQuery(genreId, movieId, id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var showtime = baseResult.GetResult<ShowtimeDto>();

            return Ok(showtime);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateShowtimeForMovie(Guid genreId, Guid movieId, [FromBody] ShowtimeForCreationDto showtime)
        {
            var baseResult = await _sender.Send(new CreateShowtimeCommand(genreId, movieId, showtime, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdShowtime = baseResult.GetResult<ShowtimeDto>();

            return CreatedAtRoute("GetShowtimeById", new { genreId = genreId, movieId = movieId, id = createdShowtime.ShowtimeId }, createdShowtime);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteShowtimeForMovie(Guid genreId, Guid movieId, Guid id)
        {
            var baseResult = await _sender.Send(new DeleteShowtimeCommand(genreId, movieId, id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateShowtimeForMovie(Guid genreId, Guid movieId, Guid id, [FromBody] ShowtimeForUpdateDto showtime)
        {
            var baseResult = await _sender.Send(new UpdateShowtimeCommand(genreId, movieId, id, showtime, MovTrackChanges: false, ShwTrackChanges: true));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}