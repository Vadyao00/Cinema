namespace Cinema.Domain.Responses
{
    public sealed class TicketNotFoundResponse : ApiNotFoundResponse
    {
        public TicketNotFoundResponse(Guid id)
            : base($"Ticket with id: {id} is not found in db.")
        {
        }
    }
}