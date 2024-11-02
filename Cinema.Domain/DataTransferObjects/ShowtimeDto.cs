namespace Cinema.Domain.DataTransferObjects
{
    public record ShowtimeDto
    {
        public Guid ShowtimeId { get; init; }

        public string? MovieTitle { get; init; }

        public DateOnly Date { get; init; }

        public TimeOnly StartTime { get; init; }

        public TimeOnly EndTime { get; init; }

        public decimal TicketPrice { get; init; }
    }
}
