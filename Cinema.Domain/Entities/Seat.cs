namespace Cinema.Domain.Entities
{
    public partial class Seat
    {
        public Guid SeatId { get; set; }

        public Guid? ShowtimeId { get; set; }

        public Guid? EventId { get; set; }

        public int? SeatNumber { get; set; }

        public bool IsOccupied { get; set; }

        public virtual Event? Event { get; set; }

        public virtual Showtime? Showtime { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}