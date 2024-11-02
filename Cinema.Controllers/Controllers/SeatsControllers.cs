using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/seats")]
    public class SeatsControllers : Controller
    {
        private readonly IServiceManager _service;

        public SeatsControllers(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetSeats()
        {
            var seats = await _service.Seat.GetAllSeatsAsync(trackChanges: false);

            return Ok(seats);
        }

        [HttpGet("{id:guid}", Name = "GetSeatById")]
        public async Task<IActionResult> GetSeat(Guid id)
        {
            var seat = await _service.Seat.GetSeatAsync(id, trackChanges: false);

            return Ok(seat);
        }

        [HttpPost("showtimes/{showtimeId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateSeatForShowtime(Guid showtimeId, [FromBody] SeatForCreationDto seat)
        {
            var createdSeat = await _service.Seat.CreateSeatForShowtimeOrEventAsync(showtimeId, null, seat, trackChanges: false);
            return CreatedAtRoute("GetSeatById", new { id = createdSeat.SeatId }, createdSeat);
        }

        [HttpPost("events/{eventId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateSeatForEvent(Guid eventId, [FromBody] SeatForCreationDto seat)
        {
            var createdSeat = await _service.Seat.CreateSeatForShowtimeOrEventAsync(null, eventId, seat, trackChanges: false);
            return CreatedAtRoute("GetSeatById", new { id = createdSeat.SeatId }, createdSeat);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSeat(Guid id)
        {
            await _service.Seat.DeleteSeatAsync(id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateSeat(Guid id, [FromBody] SeatForUpdateDto seat)
        {
            await _service.Seat.UpdateSeatAsync(id, seat, movTrackChanges: true);

            return NoContent();
        }
    }
}