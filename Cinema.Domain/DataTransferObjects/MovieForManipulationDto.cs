using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public abstract record MovieForManipulationDto
    {
        [Required(ErrorMessage = "Movie title is a required field.")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Title is 255 characters.")]
        public string? Title { get; init; }

        [Required(ErrorMessage = "Duration is a required field.")]
        public TimeOnly Duration { get; init; }

        [Required(ErrorMessage = "ProductionCompany is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the ProductionCompany is 100 characters.")]
        public string? ProductionCompany { get; init; }

        [Required(ErrorMessage = "Country is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Country is 100 characters.")]
        public string? Country { get; init; }

        [Range(0, int.MaxValue, ErrorMessage = "AgeRestriction is required and it can't be lower than 0")]
        public int? AgeRestriction { get; init; }

        [Required(ErrorMessage = "Description is a required field.")]
        [MaxLength(500, ErrorMessage = "Maximum length for the Description is 500 characters.")]
        public string? Description { get; init; }
    }
}