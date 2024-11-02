using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/employees/{employeeId}/workLogs")]
    public class WorkLogsController : Controller
    {
        private readonly IServiceManager _service;

        public WorkLogsController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetWorkLogForEmployee(Guid employeeId)
        {
            var workLogs = await _service.WorkLog.GetAllWorkLogsForEmployeeAsync(employeeId, trackChanges: false);

            return Ok(workLogs);
        }

        [HttpGet("{id:guid}", Name = "GetWorkLogById")]
        public async Task<IActionResult> GetWorkLogForEmployee(Guid employeeId, Guid id)
        {
            var workLog = await _service.WorkLog.GetWorkLogForEmployeeAsync(employeeId, id, trackChanges: false);

            return Ok(workLog);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateWorkLogForEmployee(Guid employeeId, [FromBody] WorkLogForCreationDto workLog)
        {
            var createdWorkLog = await _service.WorkLog.CreateWorkLogForEmployeeAsync(employeeId, workLog, trackChanges: false);

            return CreatedAtRoute("GetWorkLogById", new { employeeId = employeeId, id = createdWorkLog.WorkLogId }, createdWorkLog);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteWorkLog(Guid employeeId, Guid id)
        {
            await _service.WorkLog.DeleteWorkLogForEmployeeAsync(employeeId, id, trackChanges: false);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateWorkLog(Guid employeeId, Guid id, [FromBody] WorkLogForUpdateDto workLog)
        {
            await _service.WorkLog.UpdateWorkLogAsync(employeeId, id, workLog, empTrackChanges: false, wrkTrackChanges: true);

            return NoContent();
        }
    }
}