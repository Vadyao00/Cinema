namespace Cinema.Domain.DataTransferObjects
{
    public record MovieDto
    {
        public Guid MovieId { get; init; }

        public string? Title { get; init; }

        public TimeOnly Duration { get; init; }

        public string? ProductionCompany { get; init; }

        public string? Country { get; init; }

        public int? AgeRestriction { get; init; }

        public string? Description { get; init; }
        public string? GenreName { get; init; }
    }
}