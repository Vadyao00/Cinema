namespace Cinema.Domain.DataTransferObjects
{
    public record TicketDto
    {
        public Guid TicketId { get; init; }

        public int? SeatNumber { get; init; }

        public DateOnly PurchaseDate { get; init; }
    }
}