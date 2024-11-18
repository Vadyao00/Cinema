using Cinema.Application.Commands.MoviesCommands;
using Cinema.Application.Queries.MoviesQueries;
using Cinema.Controllers.Extensions;
using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Contracts.IServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/genres/{genreId}/movies")]
    [Authorize]
    public class MoviesController : ApiControllerBase
    {
        private readonly ISender _sender;

        public MoviesController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetMoviesForGenre([FromQuery]MovieParameters movieParameters, Guid genreId)
        {
            var baseResult = await _sender.Send(new GetMoviesQuery(movieParameters, genreId, TrackChanges: false));
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var (movies, metaData) = baseResult.GetResult<(IEnumerable<MovieDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(movies);
        }

        [HttpGet("{id:guid}", Name = "GetMovieById")]
        public async Task<IActionResult> GetMovieForGenre(Guid genreId, Guid id)
        {
            var baseResult = await _sender.Send(new GetMovieQuery(genreId, id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var movie = baseResult.GetResult<MovieDto>();

            return Ok(movie);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateMovieForGenre(Guid genreId, [FromBody] MovieForCreationDto movie)
        {
            var baseResult = await _sender.Send(new CreateMovieCommand(genreId, movie, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdMovie = baseResult.GetResult<MovieDto>();

            return CreatedAtRoute("GetMovieById", new { genreId = genreId, id = createdMovie.MovieId }, createdMovie);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteMovieForGenre(Guid genreId, Guid id)
        {
            var baseResult = await _sender.Send(new DeleteMovieCommand(genreId, id , TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateMovieForGenre(Guid genreId, Guid id, [FromBody] MovieForUpdateDto movie)
        {
            var baseResult = await _sender.Send(new UpdateMovieCommand(genreId, id, movie, GenrTrackChanges:false, MovTrackChanges: true));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}