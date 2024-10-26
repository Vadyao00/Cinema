namespace Cinema.Domain.DataTransferObjects
{
    public record GenreDto
    {
        public Guid GenreId { get; init; }

        public string? Name { get; init; }

        public string? Description { get; init; }
    }
}