using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public record ActorForUpdateDto : ActorForManipulationDto
    {
        [Required(ErrorMessage = "Movies is a required field.")]
        public Guid[]? MoviesIds { get; init; }
    }
}