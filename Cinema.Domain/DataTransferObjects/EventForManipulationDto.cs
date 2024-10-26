using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public record EventForManipulationDto
    {
        [Required(ErrorMessage = "Event name is a required field.")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Name is 255 characters.")]
        public string? Name { get; init; }

        [Required(ErrorMessage = "Date is a required field.")]
        public DateOnly? Date { get; init; }

        [Required(ErrorMessage = "StartTime is a required field.")]
        public TimeOnly? StartTime { get; init; }

        [Required(ErrorMessage = "EndTime is a required field.")]
        public TimeOnly? EndTime { get; init; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "TicketPrice is required and it can't be lower than 0")]
        public decimal? TicketPrice { get; init; }
    }
}