namespace Cinema.Domain.Responses
{
    public sealed class MovieNotFoundResponse : ApiNotFoundResponse
    {
        public MovieNotFoundResponse(Guid id)
            : base($"Movie with id: {id} is not found in db.")
        {
        }
    }
}