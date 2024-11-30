using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public record EventForUpdateDto : EventForManipulationDto
    {
        [Required(ErrorMessage = "Employees is a required field.")]
        public Guid[]? EmployeesIds { get; init; }
    }
}