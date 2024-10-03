namespace Cinema.Domain.Entities
{
    public partial class Showtime
    {
        public Guid ShowtimeId { get; set; }

        public Guid MovieId { get; set; }

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public decimal TicketPrice { get; set; }

        public virtual Movie Movie { get; set; } = null!;

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
