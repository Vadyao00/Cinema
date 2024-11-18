using Cinema.Application.Commands.WorkLogCommands;
using Cinema.Application.Queries.WorkLogsQueries;
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
    [Route("api/employees/{employeeId}/workLogs")]
    [Authorize]
    public class WorkLogsController : ApiControllerBase
    {
        private readonly ISender _sender;

        public WorkLogsController(ISender sender) => _sender = sender;

        [HttpGet]
        public async Task<IActionResult> GetWorkLogForEmployee([FromQuery]WorkLogParameters workLogParameters, Guid employeeId)
        {
            var baseResult = await _sender.Send(new GetWorkLogsQuery(workLogParameters, employeeId, TrackChanges: false));
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var (workLogs, metaData) = baseResult.GetResult<(IEnumerable<WorkLogDto>, MetaData)>();

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metaData));

            return Ok(workLogs);
        }

        [HttpGet("{id:guid}", Name = "GetWorkLogById")]
        public async Task<IActionResult> GetWorkLogForEmployee(Guid employeeId, Guid id)
        {
            var baseResult = await _sender.Send(new GetWorkLogQuery(employeeId, id, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var workLog = baseResult.GetResult<WorkLogDto>();

            return Ok(workLog);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateWorkLogForEmployee(Guid employeeId, [FromBody] WorkLogForCreationDto workLog)
        {
            var baseResult = await _sender.Send(new CreateWorkLogCommand(employeeId, workLog, TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdWorkLog = baseResult.GetResult<WorkLogDto>();

            return CreatedAtRoute("GetWorkLogById", new { employeeId = employeeId, id = createdWorkLog.WorkLogId }, createdWorkLog);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteWorkLog(Guid employeeId, Guid id)
        {
            var baseResult = await _sender.Send(new DeleteWorkLogCommand(employeeId, id ,TrackChanges: false));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateWorkLog(Guid employeeId, Guid id, [FromBody] WorkLogForUpdateDto workLog)
        {
            var baseResult = await _sender.Send(new UpdateWorkLogCommand(employeeId, id ,workLog, EmpTrackChanges: false, WrkTrackChanges: true));
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}