namespace Cinema.Domain.Exceptions
{
    public class ActorNotFoundException : NotFoundException
    {
        public ActorNotFoundException(Guid actorId) : base($"The actor with id: {actorId} doesn't exist in the database.")
        {
        }
    }
}