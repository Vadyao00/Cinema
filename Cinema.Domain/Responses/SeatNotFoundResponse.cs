namespace Cinema.Domain.Responses
{
    public sealed class SeatNotFoundResponse : ApiNotFoundResponse
    {
        public SeatNotFoundResponse(Guid id)
            : base($"Seat with id: {id} is not found in db.")
        {
        }
    }
}