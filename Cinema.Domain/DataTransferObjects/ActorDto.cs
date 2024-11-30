namespace Cinema.Domain.DataTransferObjects
{
    public record ActorDto
    {
        public Guid ActorId { get; init; }

        public string? Name { get; init; }

        public string? Movies { get; init; }
    }
}