namespace Cinema.Domain.Entities
{
    public partial class Movie
    {
        public Guid MovieId { get; set; }

        public string Title { get; set; } = null!;

        public TimeOnly Duration { get; set; }

        public string? ProductionCompany { get; set; }

        public string? Country { get; set; }

        public int? AgeRestriction { get; set; }

        public string? Description { get; set; }

        public Guid GenreId { get; set; }

        public virtual Genre Genre { get; set; } = null!;

        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();

        public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();
    }
}
