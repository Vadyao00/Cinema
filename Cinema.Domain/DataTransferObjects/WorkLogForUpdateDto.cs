namespace Cinema.Domain.DataTransferObjects
{
    public record WorkLogForUpdateDto : WorkLogForManipulationDto
    {
        public Guid? EmployeeId { get; init; }
    }
}