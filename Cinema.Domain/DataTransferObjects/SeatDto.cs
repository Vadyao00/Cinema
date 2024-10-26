namespace Cinema.Domain.DataTransferObjects
{
    public record SeatDto
    {
        public Guid SeatId { get; init; }

        public Guid? ShowtimeId { get; init; }

        public Guid? EventId { get; init; }

        public int? SeatNumber { get; init; }

        public bool IsOccupied { get; init; }
    }
}
