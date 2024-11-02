namespace Cinema.Domain.DataTransferObjects
{
    public record SeatDto
    {
        public Guid SeatId { get; init; }

        public string? ShowtimeName { get; init; }

        public string? EventName { get; init; }

        public int? SeatNumber { get; init; }

        public bool IsOccupied { get; init; }
    }
}