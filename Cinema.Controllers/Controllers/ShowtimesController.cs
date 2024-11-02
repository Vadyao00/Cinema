using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/genres/{genreId:guid}/movies/{movieId:guid}/showtimes")]
    public class ShowtimesController : Controller
    {
        private readonly IServiceManager _service;

        public ShowtimesController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetShowtimesForMovie(Guid genreId, Guid movieId)
        {
            var showtimes = await _service.Showtime.GetAllShowtimesAsync(genreId, movieId, trackChanges: false);

            return Ok(showtimes);
        }

        [HttpGet("{id:guid}", Name = "GetShowtimeById")]
        public async Task<IActionResult> GetShowtimeForMovie(Guid genreId, Guid movieId, Guid id)
        {
            var showtime = await _service.Showtime.GetShowtimeAsync(genreId, movieId, id, trackChanges: false);

            return Ok(showtime);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateShowtimeForMovie(Guid genreId, Guid movieId, [FromBody] ShowtimeForCreationDto showtime)
        {
            var createdShowtime = await _service.Showtime.CreateShowtimeForMovieAsync(genreId, movieId, showtime, trackChanges: false);

            return CreatedAtRoute("GetShowtimeById", new { genreId = genreId, movieId = movieId, id = createdShowtime.ShowtimeId }, createdShowtime);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteShowtimeForMovie(Guid genreId, Guid movieId, Guid id)
        {
            await _service.Showtime.DeleteShowtimeAsync(genreId, movieId, id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateShowtimeForMovie(Guid genreId, Guid movieId, Guid id, [FromBody] ShowtimeForUpdateDto showtime)
        {
            await _service.Showtime.UpdateShowtimeAsync(genreId, movieId, id, showtime, movTrackChanges: false, shwTrackChanges: true);

            return NoContent();
        }
    }
}