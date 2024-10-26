namespace Cinema.Domain.Exceptions
{
    public class TicketNotFoundException : NotFoundException
    {
        public TicketNotFoundException(Guid ticketId) : base($"The ticket with id: {ticketId} doesn't exist in the database.")
        {
        }
    }
}