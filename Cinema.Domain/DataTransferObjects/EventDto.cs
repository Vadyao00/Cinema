namespace Cinema.Domain.DataTransferObjects
{
    public record EventDto
    {
        public Guid EventId { get; init; }

        public string? Name { get; init; }

        public DateOnly Date { get; init; }

        public TimeOnly StartTime { get; init; }

        public TimeOnly EndTime { get; init; }

        public decimal TicketPrice { get; init; }
    }
}