using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public record EmployeeForUpdateDto : EmployeeForManipulationDto
    {
        [Required(ErrorMessage = "Events is a required field.")]
        public Guid[]? EventsIds { get; init; }

        [Required(ErrorMessage = "Showtimes is a required field.")]
        public Guid[]? ShowtimesIds { get; init; }
    }
}