namespace Cinema.Domain.Exceptions
{
    public class ShowtimeNotFoundException : NotFoundException
    {
        public ShowtimeNotFoundException(Guid showtimeId) : base($"The showtime with id: {showtimeId} doesn't exist in the database.")
        {
        }
    }
}