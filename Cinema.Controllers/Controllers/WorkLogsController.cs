using Cinema.Controllers.Extensions;
using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/employees/{employeeId}/workLogs")]
    public class WorkLogsController : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public WorkLogsController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetWorkLogForEmployee(Guid employeeId)
        {
            var baseResult = await _service.WorkLog.GetAllWorkLogsForEmployeeAsync(employeeId, trackChanges: false);
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var workLogs = baseResult.GetResult<IEnumerable<WorkLogDto>>();

            return Ok(workLogs);
        }

        [HttpGet("{id:guid}", Name = "GetWorkLogById")]
        public async Task<IActionResult> GetWorkLogForEmployee(Guid employeeId, Guid id)
        {
            var baseResult = await _service.WorkLog.GetWorkLogForEmployeeAsync(employeeId, id, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var workLog = baseResult.GetResult<WorkLogDto>();

            return Ok(workLog);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateWorkLogForEmployee(Guid employeeId, [FromBody] WorkLogForCreationDto workLog)
        {
            var baseResult = await _service.WorkLog.CreateWorkLogForEmployeeAsync(employeeId, workLog, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            var createdWorkLog = baseResult.GetResult<WorkLogDto>();

            return CreatedAtRoute("GetWorkLogById", new { employeeId = employeeId, id = createdWorkLog.WorkLogId }, createdWorkLog);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteWorkLog(Guid employeeId, Guid id)
        {
            var baseResult = await _service.WorkLog.DeleteWorkLogForEmployeeAsync(employeeId, id, trackChanges: false);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateWorkLog(Guid employeeId, Guid id, [FromBody] WorkLogForUpdateDto workLog)
        {
            var baseResult = await _service.WorkLog.UpdateWorkLogAsync(employeeId, id, workLog, empTrackChanges: false, wrkTrackChanges: true);
            if (!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}