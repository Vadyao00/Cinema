using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : Controller
    {
        private readonly IServiceManager _service;

        public GenresController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await _service.Genre.GetAllGenresAsync(trackChanges: false);

            return Ok(genres);
        }

        [HttpGet("{id:guid}", Name = "GenreById")]
        public async Task<IActionResult> GetGenre(Guid id)
        {
            var genre = await _service.Genre.GetGenreAsync(id, trackChanges: false);

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
            await _service.Genre.DeleteGenreAsync(id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateGenre(Guid id, [FromBody] GenreForUpdateDto genre)
        {
            await _service.Genre.UpdateGenreAsync(id, genre, trackChanges: true);

            return NoContent();
        }
    }
}