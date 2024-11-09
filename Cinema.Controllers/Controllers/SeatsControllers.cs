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
    [Route("api/seats")]
    public class SeatsControllers : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public SeatsControllers(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetSeats([FromQuery]SeatParameters seatParameters)
        {
            var baseResult = await _service.Seat.GetAllSeatsAsync(seatParameters, trackChanges: false);
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var (seats, metaData) = baseResult.GetResult<(IEnumerable<SeatDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(seats);
        }

        [HttpGet("{id:guid}", Name = "GetSeatById")]
        public async Task<IActionResult> GetSeat(Guid id)
        {
            var baseResult = await _service.Seat.GetSeatAsync(id, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var seat = baseResult.GetResult<SeatDto>();

            return Ok(seat);
        }

        [HttpPost("showtimes/{showtimeId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateSeatForShowtime(Guid showtimeId, [FromBody] SeatForCreationDto seat)
        {
            var baseResult = await _service.Seat.CreateSeatForShowtimeOrEventAsync(showtimeId, null, seat, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdSeat = baseResult.GetResult<SeatDto>();

            return CreatedAtRoute("GetSeatById", new { id = createdSeat.SeatId }, createdSeat);
        }

        [HttpPost("events/{eventId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateSeatForEvent(Guid eventId, [FromBody] SeatForCreationDto seat)
        {
            var baseResult = await _service.Seat.CreateSeatForShowtimeOrEventAsync(null, eventId, seat, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdSeat = baseResult.GetResult<SeatDto>();

            return CreatedAtRoute("GetSeatById", new { id = createdSeat.SeatId }, createdSeat);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSeat(Guid id)
        {
            var baseResult = await _service.Seat.DeleteSeatAsync(id, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateSeat(Guid id, [FromBody] SeatForUpdateDto seat)
        {
            var baseResult = await _service.Seat.UpdateSeatAsync(id, seat, seatTrackChanges: true);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}