namespace Cinema.Domain.RequestFeatures
{
    public class GenreParameters : RequestParameters
    {
        public GenreParameters() => OrderBy = "Name";
        public string? searchName { get; set; }
    }
}