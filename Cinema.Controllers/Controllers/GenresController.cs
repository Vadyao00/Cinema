using Cinema.Controllers.Extensions;
using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Cinema.Domain.RequestFeatures;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public GenresController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetGenres([FromQuery]GenreParameters genreParameters)
        {
            var baseResult = await _service.Genre.GetAllGenresAsync(genreParameters, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var (genres, metaData) = baseResult.GetResult <(IEnumerable<GenreDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(genres);
        }

        [HttpGet("{id:guid}", Name = "GenreById")]
        public async Task<IActionResult> GetGenre(Guid id)
        {
            var baseResult = await _service.Genre.GetGenreAsync(id, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var genre = baseResult.GetResult<GenreDto>();

            return Ok(genre);
        }

        [HttpPost(Name = "CreateGenre")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateGenre([FromBody] GenreForCreationDto genre)
        {
            var createdGenre = await _service.Genre.CreateGenreAsync(genre);

            return CreatedAtRoute("GenreById", new { id = createdGenre.GenreId }, createdGenre);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGenre(Guid id)
        {
            var baseResult = await _service.Genre.DeleteGenreAsync(id, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateGenre(Guid id, [FromBody] GenreForUpdateDto genre)
        {
            var baseResult = await _service.Genre.UpdateGenreAsync(id, genre, trackChanges: true);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}