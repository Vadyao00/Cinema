namespace Cinema.Domain.Responses
{
    public sealed class ShowtimeNotFoundResponse: ApiNotFoundResponse
    {
        public ShowtimeNotFoundResponse(Guid id)
            : base($"Showtime with id: {id} is not found in db.")
        {
        }
    }
}