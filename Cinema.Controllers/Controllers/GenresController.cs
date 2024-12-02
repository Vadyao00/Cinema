using Cinema.Application.Commands.GenresCommands;
using Cinema.Application.Queries.GenresQueries;
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
    [Route("api/genres")]
    [Authorize]
    public class GenresController : ApiControllerBase
    {
        private readonly ISender _sender;

        public GenresController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetGenres([FromQuery]GenreParameters genreParameters)
        {
            var baseResult = await _sender.Send(new GetGenresQuery(genreParameters, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var (genres, metaData) = baseResult.GetResult <(IEnumerable<GenreDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(genres);
        }

        [HttpGet("withoutmeta")]
        public async Task<IActionResult> GetAllGenres()
        {
            var baseResult = await _sender.Send(new GetAllGenresQuery(TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var genres = baseResult.GetResult<IEnumerable<GenreDto>>();

            return Ok(genres);
        }

        [HttpGet("{id:guid}", Name = "GenreById")]
        public async Task<IActionResult> GetGenre(Guid id)
        {
            var baseResult = await _sender.Send(new GetGenreQuery(id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var genre = baseResult.GetResult<GenreDto>();

            return Ok(genre);
        }

        [HttpPost(Name = "CreateGenre")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateGenre([FromBody] GenreForCreationDto genre)
        {
            var createdGenre = await _sender.Send(new CreateGenreCommand(genre));

            return CreatedAtRoute("GenreById", new { id = createdGenre.GenreId }, createdGenre);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteGenre(Guid id)
        {
            var baseResult = await _sender.Send(new DeleteGenreCommand(id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateGenre(Guid id, [FromBody] GenreForUpdateDto genre)
        {
            var baseResult = await _sender.Send(new UpdateGenreCommand(id, genre, TrackChanges: true));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}