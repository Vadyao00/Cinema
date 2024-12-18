﻿using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public record MovieForUpdateDto : MovieForManipulationDto
    {
        [Required(ErrorMessage = "GenreId is a required field")]
        public Guid? GenreId { get; init; }
    }
}