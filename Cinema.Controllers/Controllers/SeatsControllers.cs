using Cinema.Application.Commands.SeatsCommands;
using Cinema.Application.Queries.SeatsQueries;
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
    [Route("api/seats")]
    [Authorize]
    public class SeatsControllers : ApiControllerBase
    {
        private readonly ISender _sender;

        public SeatsControllers(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetSeats([FromQuery]SeatParameters seatParameters)
        {
            var baseResult = await _sender.Send(new GetSeatsQuery(seatParameters, TrackChanges:false));
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var (seats, metaData) = baseResult.GetResult<(IEnumerable<SeatDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(seats);
        }

        [HttpGet("withoutmeta")]
        public async Task<IActionResult> GetAllSeats()
        {
            var baseResult = await _sender.Send(new GetAllSeatsQuery(TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var seats = baseResult.GetResult<IEnumerable<SeatDto>>();

            return Ok(seats);
        }

        [HttpGet("{id:guid}", Name = "GetSeatById")]
        public async Task<IActionResult> GetSeat(Guid id)
        {
            var baseResult = await _sender.Send(new GetSeatQuery(id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var seat = baseResult.GetResult<SeatDto>();

            return Ok(seat);
        }

        [HttpPost("showtimes/{showtimeId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateSeatForShowtime(Guid showtimeId, [FromBody] SeatForCreationDto seat)
        {
            var baseResult = await _sender.Send(new CreateSeatCommand(showtimeId, null , seat, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdSeat = baseResult.GetResult<SeatDto>();

            return CreatedAtRoute("GetSeatById", new { id = createdSeat.SeatId }, createdSeat);
        }

        [HttpPost("events/{eventId:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateSeatForEvent(Guid eventId, [FromBody] SeatForCreationDto seat)
        {
            var baseResult = await _sender.Send(new CreateSeatCommand(null, eventId, seat, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdSeat = baseResult.GetResult<SeatDto>();

            return CreatedAtRoute("GetSeatById", new { id = createdSeat.SeatId }, createdSeat);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteSeat(Guid id)
        {
            var baseResult = await _sender.Send(new DeleteSeatCommand(id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateSeat(Guid id, [FromBody] SeatForUpdateDto seat)
        {
            var baseResult = await _sender.Send(new UpdateSeatCommand(id, seat, SeatTrackChanges: true));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}