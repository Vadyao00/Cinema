namespace Cinema.Domain.Exceptions
{
    public class WorkLogNotFoundException : NotFoundException
    {
        public WorkLogNotFoundException(Guid workLogId) : base($"The workLog with id: {workLogId} doesn't exist in the database.")
        {
        }
    }
}