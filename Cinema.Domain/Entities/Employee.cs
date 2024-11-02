namespace Cinema.Domain.Entities
{
    public partial class Employee
    {
        public Guid EmployeeId { get; set; }

        public string Name { get; set; } = null!;

        public string Role { get; set; } = null!;

        public virtual ICollection<WorkLog> WorkLogs { get; set; } = new List<WorkLog>();

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();

        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}