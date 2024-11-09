namespace Cinema.Domain.RequestFeatures
{
    public class ActorParameters : RequestParameters
    {
        public ActorParameters() => OrderBy = "Name";
        public string? searchName { get; set; }
    }
}