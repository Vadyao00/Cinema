namespace Cinema.Domain.Responses
{
    public sealed class EventNotFoundResponse : ApiNotFoundResponse
    {
        public EventNotFoundResponse(Guid id)
            : base($"Event with id: {id} is not found in db.")
        {
        }
    }
}