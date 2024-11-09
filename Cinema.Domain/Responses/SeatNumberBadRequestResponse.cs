namespace Cinema.Domain.Responses
{
    public sealed class SeatNumberBadRequestResponse : ApiBadRequestResponse
    {
        public SeatNumberBadRequestResponse() : base("Min seat number must be less than max seat number")
        {
        }
    }
}