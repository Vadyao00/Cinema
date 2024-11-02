namespace Cinema.Domain.Entities
{
    public partial class WorkLog
    {
        public Guid WorkLogId { get; set; }

        public Guid EmployeeId { get; set; }

        public int WorkExperience { get; set; }

        public DateOnly StartDate { get; set; }

        public decimal WorkHours { get; set; }

        public virtual Employee Employee { get; set; } = null!;
    }
}