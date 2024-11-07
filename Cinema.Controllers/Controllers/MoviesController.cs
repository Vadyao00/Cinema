using Cinema.Controllers.Extensions;
using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/genres/{genreId}/movies")]
    public class MoviesController : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public MoviesController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetMoviesForGenre(Guid genreId)
        {
            var baseResult = await _service.Movie.GetAllMoviesAsync(genreId, trackChanges: false);
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var movies = baseResult.GetResult<IEnumerable<MovieDto>>();

            return Ok(movies);
        }

        [HttpGet("{id:guid}", Name = "GetMovieById")]
        public async Task<IActionResult> GetMovieForGenre(Guid genreId, Guid id)
        {
            var baseResult = await _service.Movie.GetMovieAsync(genreId, id, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var movie = baseResult.GetResult<MovieDto>();

            return Ok(movie);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateMovieForGenre(Guid genreId, [FromBody] MovieForCreationDto movie)
        {
            var baseResult = await _service.Movie.CreateMovieForGenreAsync(genreId, movie, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdMovie = baseResult.GetResult<MovieDto>();

            return CreatedAtRoute("GetMovieById", new { genreId = genreId, id = createdMovie.MovieId }, createdMovie);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteMovieForGenre(Guid genreId, Guid id)
        {
            var baseResult = await _service.Movie.DeleteMovieAsync(genreId, id, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateMovieForGenre(Guid genreId, Guid id, [FromBody] MovieForUpdateDto movie)
        {
            var baseResult = await _service.Movie.UpdateMovieForGenreAsync(genreId, id, movie, genrTrackChanges: false, movTrackChanges: true);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}