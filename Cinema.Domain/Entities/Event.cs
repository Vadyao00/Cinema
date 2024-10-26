namespace Cinema.Domain.Entities
{
    public partial class Event
    {
        public Guid EventId { get; set; }

        public string Name { get; set; } = null!;

        public DateOnly Date { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public decimal TicketPrice { get; set; }

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}