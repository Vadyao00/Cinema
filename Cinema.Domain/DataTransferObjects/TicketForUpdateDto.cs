namespace Cinema.Domain.DataTransferObjects
{
    public record TicketForUpdateDto : TicketForManipulationDto
    {
        public Guid? SeatId { get; init; }
    }
}