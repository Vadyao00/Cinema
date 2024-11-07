namespace Cinema.Domain.Responses
{
    public sealed class ActorNotFoundResponse : ApiNotFoundResponse
    {
        public ActorNotFoundResponse(Guid id)
            : base($"Actor with id {id} is not found in db.")
        {
        }
    }
}