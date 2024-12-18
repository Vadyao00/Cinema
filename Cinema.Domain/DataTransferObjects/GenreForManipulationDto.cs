﻿using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public abstract record GenreForManipulationDto
    {
        [Required(ErrorMessage = "Genre name is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Name is 100 characters.")]
        public string? Name { get; init; }

        [Required(ErrorMessage = "Genre description is a required field.")]
        [MaxLength(255, ErrorMessage = "Maximum length for the Name is 255 characters.")]
        public string? Description { get; init; }
    }
}