using Cinema.Application.Commands.MoviesCommands;
using Cinema.Application.Queries.MoviesQueries;
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
    [Route("api/movies")]
    [Authorize]
    public class MoviesController : ApiControllerBase
    {
        private readonly ISender _sender;

        public MoviesController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetMovies([FromQuery]MovieParameters movieParameters)
        {
            var baseResult = await _sender.Send(new GetMoviesQuery(movieParameters, TrackChanges: false));
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var (movies, metaData) = baseResult.GetResult<(IEnumerable<MovieDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(movies);
        }

        [HttpGet("withoutmeta")]
        public async Task<IActionResult> GetAllMovies()
        {
            var baseResult = await _sender.Send(new GetAllMoviesQuery(TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var movies = baseResult.GetResult<IEnumerable<MovieDto>>();

            return Ok(movies);
        }

        [HttpGet("{id:guid}", Name = "GetMovieById")]
        public async Task<IActionResult> GetMovie(Guid id)
        {
            var baseResult = await _sender.Send(new GetMovieQuery(id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var movie = baseResult.GetResult<MovieDto>();

            return Ok(movie);
        }

        [HttpPost("{genreId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateMovieForGenre(Guid genreId, [FromBody] MovieForCreationDto movie)
        {
            var baseResult = await _sender.Send(new CreateMovieCommand(genreId, movie, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdMovie = baseResult.GetResult<MovieDto>();

            return CreatedAtRoute("GetMovieById", new {id = createdMovie.MovieId }, createdMovie);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            var baseResult = await _sender.Send(new DeleteMovieCommand(id , TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateMovie(Guid id, [FromBody] MovieForUpdateDto movie)
        {
            var baseResult = await _sender.Send(new UpdateMovieCommand(id, movie, GenrTrackChanges:false, MovTrackChanges: true));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}