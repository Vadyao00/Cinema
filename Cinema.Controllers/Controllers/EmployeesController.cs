using Cinema.Controllers.Extensions;
using Cinema.Controllers.Filters;
using Cinema.Domain.DataTransferObjects;
using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Controllers
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeesController : ApiControllerBase
    {
        private readonly IServiceManager _service;

        public EmployeesController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var baseResult = await _service.Employee.GetAllEmployeesAsync(trackChanges: false);

            var employees = baseResult.GetResult<IEnumerable<EmployeeDto>>();

            return Ok(employees);
        }

        [HttpGet("{id:guid}", Name = "EmployeeById")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var baseResult = await _service.Employee.GetEmployeeAsync(id, trackChanges: false);

            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            var employee = baseResult.GetResult<EmployeeDto>();

            return Ok(employee);
        }

        [HttpPost(Name = "CreateEmployee")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeForCreationDto employee)
        {
            var createdEmployee = await _service.Employee.CreateEmployeeAsync(employee);

            return CreatedAtRoute("EmployeeById", new { id = createdEmployee.EmployeeId }, createdEmployee);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var baseResult = await _service.Employee.DeleteEmployeeAsync(id, trackChanges: false);
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            var baseResult = await _service.Employee.UpdateEmployeeAsync(id, employee, trackChanges: true);
            if(!baseResult.Suссess)
                return ProccessError(baseResult);

            return NoContent();
        }
    }
}