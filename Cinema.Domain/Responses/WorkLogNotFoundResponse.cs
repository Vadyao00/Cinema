namespace Cinema.Domain.Responses
{
    public sealed class WorkLogNotFoundResponse : ApiNotFoundResponse
    {
        public WorkLogNotFoundResponse(Guid id)
            : base($"WorkLog with id: {id} is not found in db.")
        {
        }
    }
}