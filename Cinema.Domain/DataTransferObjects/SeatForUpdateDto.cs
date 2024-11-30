namespace Cinema.Domain.DataTransferObjects
{
    public record SeatForUpdateDto : SeatForManipulationDto
    {
        public Guid? EventId { get; init; }
        public Guid? ShowtimeId { get; init; }
    }
}