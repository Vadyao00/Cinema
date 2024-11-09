namespace Cinema.Domain.RequestFeatures
{
    public class MovieParameters : RequestParameters
    {
        public uint MinAgeRestriction { get; set; }
        public uint MaxAgeRestriction { get; set; } = int.MaxValue;

        public bool ValidAgeRestriction => MaxAgeRestriction > MinAgeRestriction;
    }
}