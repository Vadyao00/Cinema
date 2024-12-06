using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public abstract record EmployeeForManipulationDto
    {
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Name is 255 characters.")]
        public string? Name { get; init; }
        [Required(ErrorMessage = "Role is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Role is 100 characters.")]
        public string? Role { get; init; }

        [Required(ErrorMessage = "Events is a required field.")]
        public Guid[]? EventsIds { get; init; }

        [Required(ErrorMessage = "Showtimes is a required field.")]
        public Guid[]? ShowtimesIds { get; init; }
    }
}