using System.ComponentModel.DataAnnotations;
namespace Cinema.Domain.DataTransferObjects
{
    public abstract record ActorForManipulationDto
    {
        [Required(ErrorMessage = "Actor name is a required field.")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Name is 255 characters.")]
        public string? Name { get; init; }
    }
}
