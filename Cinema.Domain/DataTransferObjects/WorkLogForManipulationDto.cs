using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public abstract record WorkLogForManipulationDto
    {
        [Range(0, int.MaxValue, ErrorMessage = "WorkExperience is required and it can't be lower than 0")]
        public int WorkExperience { get; init; }

        [Required(ErrorMessage = "StartDate is a required field.")]
        public DateOnly StartDate { get; init; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "WorkHours is required and it can't be lower than 0")]
        public decimal WorkHours { get; init; }
    }
}