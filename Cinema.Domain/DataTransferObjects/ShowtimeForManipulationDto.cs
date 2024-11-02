using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public abstract record ShowtimeForManipulationDto
    {
        [Required(ErrorMessage = "Date is a required field.")]
        public DateOnly Date { get; init; }

        [Required(ErrorMessage = "StartTime is a required field.")]
        public TimeOnly StartTime { get; init; }

        [Required(ErrorMessage = "EndTime is a required field.")]
        public TimeOnly EndTime { get; init; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "TicketPrice is required and it can't be lower than 0")]
        public decimal TicketPrice { get; init; }
    }
}