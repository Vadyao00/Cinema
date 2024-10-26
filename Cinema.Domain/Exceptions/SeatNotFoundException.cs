namespace Cinema.Domain.Exceptions
{
    public class SeatNotFoundException : NotFoundException
    {
        public SeatNotFoundException(Guid seatId) : base($"The seat with id: {seatId} doesn't exist in the database.")
        {
        }
    }
}