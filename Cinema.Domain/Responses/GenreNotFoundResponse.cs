namespace Cinema.Domain.Responses
{
    public sealed class GenreNotFoundResponse : ApiNotFoundResponse
    {
        public GenreNotFoundResponse(Guid id)
            : base($"Genre with id: {id} is not found in db.")
        {
        }
    }
}