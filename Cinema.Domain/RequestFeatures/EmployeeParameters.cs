namespace Cinema.Domain.RequestFeatures
{
    public class EmployeeParameters : RequestParameters
    {
        public EmployeeParameters() => OrderBy = "Name";
        public string? searchName { get; set; }
    }
}