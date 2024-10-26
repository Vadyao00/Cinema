namespace Cinema.Domain.Exceptions
{
    public class GenreNotFoundException : NotFoundException
    {
        public GenreNotFoundException(Guid genreId) : base($"The genre with id: {genreId} doesn't exist in the database.")
        {
        }
    }
}