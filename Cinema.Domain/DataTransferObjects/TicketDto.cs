namespace Cinema.Domain.DataTransferObjects
{
    public record TicketDto
    {
        public Guid TicketId { get; init; }

        public Guid SeatId { get; init; }

        public DateOnly PurchaseDate { get; init; }

    }
}
