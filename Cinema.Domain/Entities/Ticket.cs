namespace Cinema.Domain.Entities
{
    public partial class Ticket
    {
        public Guid TicketId { get; set; }

        public Guid SeatId { get; set; }

        public DateOnly PurchaseDate { get; set; }

        public virtual Seat Seat { get; set; } = null!;
    }
}
