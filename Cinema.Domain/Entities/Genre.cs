namespace Cinema.Domain.Entities
{
    public partial class Genre
    {
        public Guid GenreId { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}