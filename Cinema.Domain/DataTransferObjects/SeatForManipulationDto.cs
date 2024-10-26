using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public abstract record SeatForManipulationDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "SeatNumber is required and it can't be lower than 1")]
        public int? SeatNumber { get; init; }

        [Required(ErrorMessage = "IsOccupied is a required field.")]
        public bool IsOccupied { get; init; }
    }
}
