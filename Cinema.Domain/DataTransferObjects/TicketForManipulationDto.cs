using System.ComponentModel.DataAnnotations;

namespace Cinema.Domain.DataTransferObjects
{
    public abstract record TicketForManipulationDto
    {
        [Required(ErrorMessage = "PurchaseDate is a required field.")]
        public DateOnly PurchaseDate { get; init; }
    }
}
