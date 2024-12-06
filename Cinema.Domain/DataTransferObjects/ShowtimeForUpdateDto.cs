using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public record ShowtimeForUpdateDto : ShowtimeForManipulationDto
    {
        [Required(ErrorMessage = "MovieId is a required field.")]
        public Guid? MovieId { get; init; }
    }
}