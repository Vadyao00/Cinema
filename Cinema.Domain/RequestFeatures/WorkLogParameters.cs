namespace Cinema.Domain.RequestFeatures
{
    public class WorkLogParameters : RequestParameters
    {
        public WorkLogParameters() => OrderBy = "StartDate";

        public string? searchName { get; set; }
    }
}